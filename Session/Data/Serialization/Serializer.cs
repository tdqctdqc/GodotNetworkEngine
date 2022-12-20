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
    private static Dictionary<Type, IEntityMeta> _serializableMetas;
    public static IEntityMeta GetEntityMeta(Type type) => _serializableMetas[type];

    public static EntityMeta<T> GetEntityMeta<T>() where T : Entity
    {
        return (EntityMeta<T>)_serializableMetas[typeof(T)];
    }
    public static void Setup()
    {
        var reference = nameof(EntityMeta<Entity>.ForReference);
        _serializableMetas = new Dictionary<Type, IEntityMeta>();
        var entityTypes = Assembly.GetExecutingAssembly().GetConcreteTypesOfType<Entity>();
        var metaTypes = typeof(EntityMeta<>);
        foreach (var entityType in entityTypes)
        {
            var genericMeta = metaTypes.MakeGenericType(entityType);
            var constructor = genericMeta.GetConstructors()[0];
            var meta = constructor.Invoke(null);
            _serializableMetas.Add(entityType, (IEntityMeta)meta);
        }
    }
}
