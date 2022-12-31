using Godot;
using System;
[EntityVariable]
[EntityString]
public class EntityString 
{
    public string Name { get; private set; }
    public string Value { get; private set; }
    public int EntityId { get; private set; }
    public EntityString(string value, int entityId, string name)
    {
        //only want to call this for deserialization and from construct
        Name = name;
        Value = value;
        EntityId = entityId;
    }
    public static EntityString Construct(string value, Entity entity, string name)
    {
        return new EntityString(value, entity.Id.Value, name);
    }
    public void Update(StrongWriteKey key, string newValue, IRepo repo, IServer server)
    {
        Value = newValue;
        repo.RaiseValueChangedNotice(Name, EntityId, key);
        if (key is HostWriteKey hKey)
        {
            var update = EntityVarUpdate.Encode<string>(Name, EntityId, newValue, hKey);
            ((HostServer)server).QueueUpdate(update);
        }
    }
    public static void ReceiveUpdate(EntityString str, ServerWriteKey key, string newValueJson, IRepo repo)
    {
        str.Value = newValueJson;
        repo.RaiseValueChangedNotice(str.Name, str.EntityId, key);
    }
    public void ProcedureSet(ProcedureWriteKey key, string newValue)
    {
        Value = newValue;
    }
    public static string Serialize(EntityString es)
    {
        return es.Value;
    }
    public static EntityString Deserialize(string json, string name, Entity entity)
    {
        return Construct(json, entity, name);
    }
}
