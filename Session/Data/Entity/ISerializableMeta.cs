using Godot;
using System;
using System.Collections.Generic;

public interface ISerializableMeta
{
    IReadOnlyList<string> FieldNames { get; }
    IReadOnlyList<Type> FieldTypes { get; }
    Func<StrongWriteKey, string, Serializable> Constructor { get; }

}
