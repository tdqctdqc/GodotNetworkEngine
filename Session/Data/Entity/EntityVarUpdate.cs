using Godot;
using System;
using System.Collections.Generic;

public sealed class EntityVarUpdate : IUpdate
{
    string IUpdate.UpdateType => UpdateType;
    public static string UpdateType = "Var";
    public string FieldName { get; private set; }
    public int EntityId { get; private set; }
    public string NewVal { get; private set; }
    public static EntityVarUpdate Encode<TValue>(string fieldName, int entityId, TValue newVal, HostWriteKey key)
    {
        return new EntityVarUpdate(fieldName, entityId,
            System.Text.Json.JsonSerializer.Serialize<TValue>(newVal), key);
    }
    private EntityVarUpdate(string fieldName, int entityId, string newVal, HostWriteKey key)
    {
        FieldName = fieldName;
        EntityId = entityId;
    }
    public static void DeserializeAndEnact(string json, ServerWriteKey key)
    {
        var update = System.Text.Json.JsonSerializer.Deserialize<EntityVarUpdate>(json);
        var entity = Game.I.Session.Data[update.EntityId];
        var meta = entity.GetMeta();
        meta.UpdateEntityVar(update.FieldName, entity, key, update.NewVal, null);
    }
}
