using Godot;
using System;

public sealed class EntityVarUpdate : IUpdate
{
    public static EntityVarUpdate Encode<TValue>(string fieldName, int entityId, Domain domain, TValue newVal, HostWriteKey key)
    {
        return new EntityVarUpdate(fieldName, entityId, domain.GetType(), System.Text.Json.JsonSerializer.Serialize<TValue>(newVal), key);
    }

    private EntityVarUpdate(string fieldName, int entityId, Type domainType, string newVal, HostWriteKey key)
    {
    }
    private EntityVarUpdate(string json) { }
}
