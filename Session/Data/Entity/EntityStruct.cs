using Godot;
using System;
[EntityVariable]
public struct EntityStruct<TValue> where TValue: struct
{
    public string Name { get; private set; }
    public TValue Value { get; private set; }
    public int EntityId { get; private set; }
    private EntityStruct(TValue value, int entityId, string name)
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
    public void Update(StrongWriteKey key, TValue newValue, IRepo repo, IServer server)
    {
        Value = newValue;
        repo.RaiseValueChangedNotice(Name, EntityId, key);
        if (key is HostWriteKey hKey)
        {
            var update = EntityVarUpdate.Encode<TValue>(Name, EntityId, repo.Domain, newValue, hKey);
            ((HostServer)server).QueueUpdate(update);
        }
    }
    public void ProcedureSet(ProcedureWriteKey key, TValue newValue)
    {
        Value = newValue;
    }
    public static string Serialize(EntityStruct<TValue> es)
    {
        return System.Text.Json.JsonSerializer.Serialize<TValue>(es.Value);
    }

    public static EntityStruct<TValue> Deserialize(string json, string name, Entity entity)
    {
        var value = System.Text.Json.JsonSerializer.Deserialize<TValue>(json);
        return Construct(value, entity, name);
    }
}
public class EntityVariableAttribute : Attribute
{
    /*
    Must implement
    public static TVariable<TValue> Construct(TValue value, Entity entity, string name)
    public void Update(StrongWriteKey key, TValue newValue, IRepo repo, IServer server)
    public void ProcedureSet(ProcedureWriteKey key, TValue newValue)
    public static string Serialize(TVariable<TValue> es)
    public static TVariable<TValue> Deserialize(string json, string name, Entity entity)
    
    Shouldn't set TValue from outside the constructor, Update method, or procedure set method
    Shouldn't set fields of the TValue outside an entity procedure 
    Procedure set method should only be called inside entity procedures
    */
}
