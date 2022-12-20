using Godot;
using System;
using System.Collections.Generic;

public interface ISerializableMeta
{
    IReadOnlyList<string> FieldNames { get; }
    IReadOnlyList<Type> FieldTypes { get; }
    Serializable Deserialize(string json);
    void Initialize(Serializable serializable, string json);
    string SerializeSerializable(Serializable serializable);
}
