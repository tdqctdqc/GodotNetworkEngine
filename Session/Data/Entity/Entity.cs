using Godot;
using System;
using System.Collections.Generic;

public abstract class Entity : Serializable
{
    public int Id => Get<int>(nameof(Id));
    
    protected Entity(int id, HostWriteKey key) : base()
    {
        SetInner<int>(id, nameof(Id), key);
    }
    protected Entity(StrongWriteKey key, string json) : base(key, json) { }
    
    public void Set<T>(T val, string name, StrongWriteKey key, Domain domain, IServer server)
    {
        SetInner(val, name, key);
        if (key is HostWriteKey hKey)
        {
            var update = EntityUpdate.Encode<T>(name, Id, domain, val, hKey);
            ((HostServer)server).QueueUpdate(update);
        }
    }
}
