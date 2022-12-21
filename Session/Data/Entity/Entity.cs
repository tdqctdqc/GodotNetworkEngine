using Godot;
using System;
using System.Collections.Generic;

public abstract class Entity
{
    public EntityStruct<int> Id { get; private set; }
    protected IEntityMeta _meta => Serializer.GetEntityMeta(GetType());


    protected Entity(int id, HostWriteKey key) : base()
    {
        Id = EntityStruct<int>.Construct(id, this, nameof(Id));
    }
    protected Entity(string json)
    {        
        _meta.Initialize(this, json);
    }
}
