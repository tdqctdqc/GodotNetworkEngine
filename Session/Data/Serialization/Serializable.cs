using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;

public abstract class Serializable
{
    private Dictionary<string, object> _values;
    protected ISerializableMeta _meta => Serializer.GetSerializableMeta(GetType());

    protected Serializable()
    {
        _values = new Dictionary<string, object>();
    }
    protected Serializable(StrongWriteKey key, string json)
    {
        _values = new Dictionary<string, object>();
        var valueJsons = System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
        for (var i = 0; i < _meta.FieldTypes.Count; i++)
        {
            var fieldType = _meta.FieldTypes[i];
            object val = Serializer.Deserialize(valueJsons[i], fieldType, key);
            _values.Add(_meta.FieldNames[i], val);
        }
    }
    protected void SetInner<T>(T val, string name, StrongWriteKey key)
    {
        _values[name] = val;
    }
    protected T Get<T>(string name)
    {
        return (T) _values[name];
    }
    public string Serialize()
    {
        var jsonArray = new JsonArray();
        var valueNames = _meta.FieldNames;
        for (int i = 0; i < valueNames.Count; i++)
        {
            jsonArray.Add(Serializer.Serialize(_values[valueNames[i]]));
        }
        return jsonArray.ToJsonString();
    }
    
    public static Serializable Deserialize(string json, Type serializableType, StrongWriteKey key)
    {
        return Serializer.GetSerializableMeta(serializableType).Constructor(key, json);
    }
}
