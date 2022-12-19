using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class SerializableMeta<T> : ISerializableMeta
{
    public IReadOnlyList<string> FieldNames => _fieldNames;
    private List<string> _fieldNames;
    public IReadOnlyList<Type> FieldTypes => _fieldTypes;
    private List<Type> _fieldTypes;
    public Func<StrongWriteKey, string, Serializable> Constructor { get; private set; }

    public void ForReference()
    {
        return;
        new SerializableMeta<T>();
    }
    public SerializableMeta()
    {
        var entityType = typeof(T);
        var properties = entityType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        _fieldNames = properties.Select(p => p.Name).ToList();
        _fieldTypes = properties.Select(p => p.PropertyType).ToList();
        SetConstructor(entityType);
    }

    private void SetConstructor(Type entityType)
    {
        var constructor = entityType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
            .Where(m => m.Name == "DeserializeConstructor")
            .Where(m => m.ReturnType == entityType)
            .First();
        Constructor = (Func<StrongWriteKey, string, Serializable>)Delegate.CreateDelegate(typeof(Func<StrongWriteKey, string, Serializable>), constructor);
    }
    
}
