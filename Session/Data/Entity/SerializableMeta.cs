using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;

public class SerializableMeta<T> : ISerializableMeta where T : Serializable
{
    public IReadOnlyList<string> FieldNames => _fieldNames;
    private List<string> _fieldNames;
    public IReadOnlyList<Type> FieldTypes => _fieldTypes;

    private List<Type> _fieldTypes;
    private Dictionary<string, Func<string, object>> _fieldDeserializers; 
    private Dictionary<string, Func<object, string>> _fieldSerializers; 
    private Dictionary<string, Func<T, object>> _fieldGetters; 
    private Dictionary<string, Action<T, object>> _fieldInitializers;
    private Func<string, Serializable> _deserializer;

    public void ForReference()
    {
        return;
        new SerializableMeta<T>();
    }
    public SerializableMeta()
    {
        var serializableType = typeof(T);
        //bc with generic parameters it will not capture all the classes
        if (serializableType.ContainsGenericParameters) throw new Exception(); 
        
        //don't want concrete entity classes having descendents 
        if (typeof(Entity).IsAssignableFrom(serializableType) && serializableType.IsSealed == false) 
            throw new Exception();
        
        var properties = serializableType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        _fieldNames = properties.Select(p => p.Name).ToList();
        _fieldTypes = properties.Select(p => p.PropertyType).ToList();
        _fieldDeserializers = new Dictionary<string, Func<string, object>>();
        _fieldSerializers = new Dictionary<string, Func<object, string>>();
        _fieldGetters = new Dictionary<string, Func<T, object>>();
        _fieldInitializers = new Dictionary<string, Action<T, object>>();

        foreach (var propertyInfo in properties)
        {
            SetFuncs(serializableType, propertyInfo);
        }
        SetConstructor(serializableType);
    }

    private void SetFuncs(Type serializableType, PropertyInfo p)
    {
        //could do custom serialization for entity var etc
        //to avoid space overhead of another level of abstraction in serialization 
        var name = p.Name;
        Func<string, object> fieldDeserializer; 
        Func<object, string> fieldSerializer;
        
        
        // var fieldGetter = p.GetGetMethod().MakeInstanceMethodDelegate<Func<T, object>>();
        
        //still have to initialize values inside entityVariable
        //and compress them
        var fieldInitializer = MakeSetter(p);
        var fieldGetter = MakeGetter(p);

        if (typeof(Serializable).IsAssignableFrom(p.PropertyType))
        {
            //disallow for entities to have serializable fields?
            fieldDeserializer = Serializer.GetSerializableMeta(p.PropertyType).Deserialize;
            fieldSerializer = o => Serializer.GetSerializableMeta(p.PropertyType).SerializeSerializable((Serializable)o);
        }
        else
        {
            fieldSerializer = Serializer.Serialize;
            fieldDeserializer = json => Serializer.Deserialize(json, p.PropertyType);
        }
        _fieldDeserializers[name] = fieldDeserializer;
        _fieldSerializers[name] = fieldSerializer;
        _fieldGetters[name] = fieldGetter;
        _fieldInitializers[name] = fieldInitializer;
    }

    private Action<T,object> MakeSetter(PropertyInfo p)
    {
        var inner = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .First(mi => mi.Name == nameof(SerializableMeta<T>.MakeSetterInner)
                         && mi.GetGenericArguments().Length == 1)
            .MakeGenericMethod(p.PropertyType);
        return (Action<T,object>)inner.Invoke(null, new object[] {p});
    }
    private static Action<T,object> MakeSetterInner<TProperty>(PropertyInfo p)
    {
        var setMethodType =
            ReflectionExt.MakeCustomDelegateType(typeof(Action<,>), new Type[] {typeof(T), p.PropertyType});
        var fieldInitializer = (Action<T,TProperty>)p.GetSetMethod().MakeInstanceMethodDelegate(setMethodType);
        return (t, o) => fieldInitializer(t, (TProperty) o);
    }

    private Func<T, object> MakeGetter(PropertyInfo p)
    {
        var inner = GetType()
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
            .First(mi => mi.Name == nameof(SerializableMeta<T>.MakeGetterInner)
                         && mi.GetGenericArguments().Length == 1)
            .MakeGenericMethod(p.PropertyType);
        return (Func<T,object>)inner.Invoke(null, new object[] {p});
    }

    private static Func<T, object> MakeGetterInner<TProperty>(PropertyInfo p)
    {
        var getMethodType = //System.Linq.Expressions.GetActionType(typeof(T), p.PropertyType);
            ReflectionExt.MakeCustomDelegateType(typeof(Func<,>), new Type[] {typeof(T), p.PropertyType});
        var fieldInitializer = (Func<T,TProperty>)p.GetGetMethod().MakeInstanceMethodDelegate(getMethodType);
        return t => fieldInitializer(t);
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

    public void Initialize(Serializable serializable, string json)
    {
        var valJsons = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
        for (int i = 0; i < valJsons.Count; i++)
        {
            var fieldName = _fieldNames[i];
            GD.Print(fieldName);
            var value = _fieldDeserializers[fieldName](valJsons[i]);
            GD.Print(valJsons[i]);
            _fieldInitializers[fieldName]((T)serializable, value);
        }
    }

    public string SerializeSerializable(Serializable serializable)
    {
        var jsonArray = new JsonArray();
        for (int i = 0; i < _fieldNames.Count; i++)
        {
            var fieldName = _fieldNames[i];
            var getter = _fieldGetters[fieldName];
            jsonArray.Add(Serializer.Serialize(getter((T)serializable)));
        }
        return jsonArray.ToJsonString();
    }
}
