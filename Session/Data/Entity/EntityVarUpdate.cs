using Godot;
using System;

public sealed class EntityVarUpdate : Serializable, IUpdate
{
    // public string FieldName => Get<string>(nameof(FieldName)); 
    //
    // public int EntityId => Get<int>(nameof(EntityId));
    //
    // public Type DomainType => Get<Type>(nameof(DomainType));
    // public string NewVal => Get<string>(nameof(NewVal)); 

    public static EntityVarUpdate Encode<TValue>(string fieldName, int entityId, Domain domain, TValue newVal, HostWriteKey key)
    {
        return new EntityVarUpdate(fieldName, entityId, domain.GetType(), Serializer.Serialize<TValue>(newVal), key);
    }

    private EntityVarUpdate(string fieldName, int entityId, Type domainType, string newVal, HostWriteKey key)
    {
        // SetInner(fieldName, nameof(FieldName), key);
        // SetInner(entityId, nameof(EntityId), key);
        // SetInner(newVal, nameof(NewVal), key);
        // SetInner(domainType, nameof(DomainType), key);
    }
    private static EntityVarUpdate DeserializeConstructor(string json)
    {
        return new EntityVarUpdate(json);
    }
    private EntityVarUpdate(string json) : base(json) { }
}
