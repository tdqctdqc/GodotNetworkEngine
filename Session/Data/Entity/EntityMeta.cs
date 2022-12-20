using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using Mono.Reflection;

public class EntityMeta<T> : IEntityMeta where T : Entity
{
    public IReadOnlyList<string> FieldNames => _fieldNames;
    private List<string> _fieldNames;
    public IReadOnlyList<Type> FieldTypes => _fieldTypes;
    private List<Type> _fieldTypes;
    
    //args are val, name, entityId
    private Dictionary<string, Func<string, string, T, object>> _fieldDeserializers; 
    
    private Dictionary<string, Func<object, string>> _fieldSerializers; 
    private Dictionary<string, Func<T, object>> _fieldGetters; 
    private Dictionary<string, Action<T, object>> _fieldInitializers;
    private Func<string, Serializable> _deserializer;

    public void ForReference()
    {
        return;
        new EntityMeta<T>();
    }
    public EntityMeta()
    {
        var entityType = typeof(T);
        GD.Print(entityType.Name);

        //bc with generic parameters it will not capture all the classes
        if (entityType.ContainsGenericParameters) throw new Exception(); 
        
        //don't want concrete entity classes having descendents 
        if (entityType.IsSealed == false) 
            throw new Exception();
        
        var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        _fieldNames = properties.Select(p => p.Name).ToList();
        _fieldTypes = properties.Select(p => p.PropertyType).ToList();
        _fieldDeserializers = new Dictionary<string, Func<string, string, T, object>>();
        _fieldSerializers = new Dictionary<string, Func<object, string>>();
        _fieldGetters = new Dictionary<string, Func<T, object>>();
        _fieldInitializers = new Dictionary<string, Action<T, object>>();

        foreach (var propertyInfo in properties)
        {
            SetFuncs(entityType, propertyInfo);
        }
        SetConstructor(entityType);
    }

    private void SetFuncs(Type serializableType, PropertyInfo p)
    {
        var name = p.Name;
        GD.Print(name);
        var propType = p.PropertyType;
        if (propType.HasAttribute<EntityVariableAttribute>() == false) throw new Exception();
        
        _fieldDeserializers[name] = MakeDeserializer(p);
        _fieldSerializers[name] = MakeSerializer(p);
        _fieldGetters[name] = MakeGetter(p);
        _fieldInitializers[name] = MakeSetter(p);
    }
    private Func<string, string, T, object> MakeDeserializer(PropertyInfo p)
    {
        var inner = typeof(EntityMeta<T>).GetMethod(nameof(MakeDeserializerInner),
            BindingFlags.Static | BindingFlags.NonPublic);
        var innerGeneric = inner.MakeGenericMethod(p.PropertyType);
        return (Func<string, string, T, object>) innerGeneric.Invoke(null, new []{p});
    }
    private static Func<string, string, T, object> MakeDeserializerInner<TProperty>(PropertyInfo p)
    {
        var serializeMi = p.PropertyType.GetMethod("Deserialize", BindingFlags.Static | BindingFlags.Public);
        var del = serializeMi.MakeStaticMethodDelegate<Func<string, string, T, TProperty>>();
        return (json, name, entity) => del.Invoke(json, name, entity);
    }
    private Func<object, string> MakeSerializer(PropertyInfo p)
    {
        var inner = typeof(EntityMeta<T>).GetMethod(nameof(MakeSerializerInner),
            BindingFlags.Static | BindingFlags.NonPublic);
        var innerGeneric = inner.MakeGenericMethod(p.PropertyType);
        return (Func<object, string>) innerGeneric.Invoke(null, new []{p});
    }
    private static Func<object, string> MakeSerializerInner<TProperty>(PropertyInfo p)
    {
        var serializeMi = p.PropertyType.GetMethod("Serialize", BindingFlags.Static | BindingFlags.Public);
        var del = serializeMi.MakeStaticMethodDelegate<Func<TProperty, string>>();
        return o => del.Invoke((TProperty) o);
    }
    private Action<T, object> MakeSetter(PropertyInfo p)
    {
        var inner = typeof(EntityMeta<T>).GetMethod(nameof(MakeSetterInner),
            BindingFlags.Static | BindingFlags.NonPublic);
        var innerGeneric = inner.MakeGenericMethod(p.PropertyType);
        return (Action<T, object>) innerGeneric.Invoke(null, new[] {p});
    }
    private static Action<object, object> MakeSetterInner<TProperty>(PropertyInfo p)
    {
        var mi = p.DeclaringType.GetProperty(p.Name).GetSetMethod(true);
        var setterType = ReflectionExt.MakeCustomDelegateType(typeof(Action<,>), new[] {typeof(T), p.PropertyType});
        var setter = (Action<T, TProperty>)mi.MakeInstanceMethodDelegate(setterType);
        return (entity, variable) => setter((T)entity, (TProperty)variable);
    }
    private Func<T, object> MakeGetter(PropertyInfo p)
    {
        var inner = typeof(EntityMeta<T>).GetMethod(nameof(MakeGetterInner),
            BindingFlags.Static | BindingFlags.NonPublic);
        var innerGeneric = inner.MakeGenericMethod(p.PropertyType);
        return (Func<T, object>) innerGeneric.Invoke(null, new[] {p});
    }
    private static Func<T, object> MakeGetterInner<TProperty>(PropertyInfo p)
    {
        var mi = p.GetGetMethod();
        var getterType = ReflectionExt.MakeCustomDelegateType(typeof(Func<,>), new[] {typeof(T), p.PropertyType});
        var getter = (Func<T, TProperty>)mi.MakeInstanceMethodDelegate(getterType);
        return entity => getter(entity);
    }
    private void SetConstructor(Type serializableType)
    {
        var constructor = serializableType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == "DeserializeConstructor")
            .Where(m => m.ReturnType == serializableType)
            .First();
        _deserializer = constructor.MakeStaticMethodDelegate<Func<string, Serializable>>();
    }

    public Serializable Deserialize(string json)
    {
        return _deserializer(json);
    }

    public string Serialize(T t)
    {
        var jsonArray = new System.Text.Json.Nodes.JsonArray();
        for (int i = 0; i < _fieldNames.Count; i++)
        {
            var fieldName = _fieldNames[i];
            var field = _fieldGetters[fieldName](t);
            jsonArray.Add(_fieldSerializers[fieldName](field));
        }

        return jsonArray.ToJsonString();
    }
    public void Initialize(Serializable serializable, string json)
    {
        var t = (T) serializable;
        var valJsons = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
        for (int i = 0; i < valJsons.Count; i++)
        {
            var fieldName = _fieldNames[i];
            GD.Print(fieldName);
            var value = _fieldDeserializers[fieldName](valJsons[i], fieldName, t);
            GD.Print(valJsons[i]);
            _fieldInitializers[fieldName](t, value);
        }
    }
}
