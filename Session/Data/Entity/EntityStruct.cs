using Godot;
using System;
[EntityVariable]
public struct EntityStruct<TValue> where TValue: struct
{
    public string Name { get; private set; }
    public TValue Value { get; private set; }
    public int EntityId { get; private set; }
    public EntityStruct(TValue value, int entityId, string name)
    {
        //only want to call this for deserialization and from construct
        Name = name;
        Value = value;
        EntityId = entityId;
    }

    public static EntityStruct<TValue> Construct(TValue value, Entity entity, string name)
    {
        return new EntityStruct<TValue>(value, entity.Id.Value, name);
    }

    public void Set(StrongWriteKey key, TValue newValue, IRepo repo, IServer server)
    {
        Value = newValue;
        repo.RaiseValueChangedNotice(Name, EntityId, key);
        if (key is HostWriteKey hKey)
        {
            var update = EntityVarUpdate.Encode<TValue>(Name, EntityId, repo.Domain, newValue, hKey);
            ((HostServer)server).QueueUpdate(update);
        }
    }
}
public class EntityVariableAttribute : Attribute
{
    
}
