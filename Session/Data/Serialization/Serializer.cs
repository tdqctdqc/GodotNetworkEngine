using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Nodes;

public class Serializer
{
    private static Dictionary<Type, Func<string, object>> _deserializers;
    private static Dictionary<Type, Func<object, string>> _serializers;
    private static Dictionary<Type, ISerializableMeta> _serializableMetas;
    public static ISerializableMeta GetSerializableMeta(Type type) => _serializableMetas[type];
    public static string Serialize<T>(T t)
    {
        if (t is Serializable s)
        {
            return s.Serialize();
        }
        if (t is Type type)
        {
            return type.Name;
        }
        return System.Text.Json.JsonSerializer.Serialize(t);
    }

    public static T Deserialize<T>(string json, StrongWriteKey key)
    {
        return (T) Deserialize(json, typeof(T), key);
    }
    public static object Deserialize(string json, Type type, StrongWriteKey key)
    {
        if(typeof(Serializable).IsAssignableFrom(type)) 
        {
            return Serializable.Deserialize(json, type, key);
        }
        if (typeof(Type).IsAssignableFrom(type))
        {
            return Type.GetType(json);
        }
        return System.Text.Json.JsonSerializer.Deserialize(json, type);
    }
    public static void Setup()
    {
        var reference = nameof(SerializableMeta<object>.ForReference);
        _serializableMetas = new Dictionary<Type, ISerializableMeta>();
        var serializableTypes = Assembly.GetExecutingAssembly().GetConcreteTypesOfType<Serializable>();
        var serializableMetaType = typeof(SerializableMeta<>);
        foreach (var serializableType in serializableTypes)
        {
            var genericMeta = serializableMetaType.MakeGenericType(serializableType);
            var constructor = genericMeta.GetConstructors()[0];
            var meta = constructor.Invoke(null);
            _serializableMetas.Add(serializableType, (ISerializableMeta)meta);
        }
    }
}
