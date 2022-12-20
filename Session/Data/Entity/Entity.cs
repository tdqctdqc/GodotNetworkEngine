using Godot;
using System;
using System.Collections.Generic;

public abstract class Entity : Serializable
{
    public EntityStruct<int> Id { get; private set; }

    protected Entity(int id, HostWriteKey key) : base()
    {
        Id = EntityStruct<int>.Construct(id, this, nameof(Id));
    }
    protected Entity(string json) : base(json) { }
}
