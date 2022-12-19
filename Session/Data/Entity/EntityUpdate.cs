using Godot;
using System;

public class EntityUpdate : Serializable, IUpdate
{
    public string FieldName => Get<string>(nameof(FieldName)); 
    
    public int EntityId => Get<int>(nameof(EntityId));
    
    public Type DomainType => Get<Type>(nameof(DomainType));
    public string NewVal => Get<string>(nameof(NewVal)); 

    public static EntityUpdate Encode<T>(string fieldName, int entityId, Domain domain, T newVal, HostWriteKey key)
    {
        return new EntityUpdate(fieldName, entityId, domain.GetType(), Serializer.Serialize<T>(newVal), key);
    }

    private EntityUpdate(string fieldName, int entityId, Type domainType, string newVal, HostWriteKey key)
    {
        SetInner(fieldName, nameof(FieldName), key);
        SetInner(entityId, nameof(EntityId), key);
        SetInner(newVal, nameof(NewVal), key);
        SetInner(domainType, nameof(DomainType), key);
    }
    private static EntityUpdate DeserializeConstructor(StrongWriteKey key, string json)
    {
        return new EntityUpdate(key, json);
    }
    private EntityUpdate(StrongWriteKey key, string json) : base(key, json) { }
}
