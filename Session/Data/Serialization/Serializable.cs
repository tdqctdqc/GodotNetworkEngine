using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Nodes;

public abstract class Serializable
{
    protected ISerializableMeta _meta => Serializer.GetSerializableMeta(GetType());

    protected Serializable()
    {
    }
    protected Serializable(string json)
    {
        _meta.Initialize(this, json);
    }
}
