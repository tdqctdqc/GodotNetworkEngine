using Godot;
using System;
using System.Text.Json;

[EntityVariable]

public class EntityStruct<TValue> where TValue: struct
{
    public string Name { get; private set; }
    public TValue Value { get; private set; }
    public int EntityId { get; private set; }
    private EntityStruct(TValue value, int entityId, string name)
    {
        //only want to call this for deserialization and from construct
        //TODO check that TValue doesnt have class fields or struct fields with class fields etc
        Name = name;
        Value = value;
        EntityId = entityId;
    }
    public static EntityStruct<TValue> Construct(TValue value, Entity entity, string name)
    {
        return new EntityStruct<TValue>(value, entity.Id.Value, name);
    }
    public void Update(HostWriteKey key, TValue newValue, IRepo repo, IServer server)
    {
        Value = newValue;
        repo.RaiseValueChangedNotice(Name, EntityId, key);
        var update = EntityVarUpdate.Encode<TValue>(Name, EntityId, newValue, key);
        ((HostServer)server).QueueUpdate(update);
    }
    public static void ReceiveUpdate(EntityStruct<TValue> str, ServerWriteKey key, string newValueJson, IRepo repo)
    {
        GD.Print("new value json is " + newValueJson);
        var value = Serializer.Deserialize<TValue>(newValueJson);
        GD.Print("new value is " + value);
        str.Value = value;
        repo?.RaiseValueChangedNotice(str.Name, str.EntityId, key);
    }
    public void SetByProcedure(ProcedureWriteKey key, TValue newValue)
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
