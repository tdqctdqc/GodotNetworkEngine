using Godot;
using System;

public struct EntityRef<TRef> where TRef : Entity
{
    public string Name { get; private set; }
    public int HostId { get; private set; }
    public int RefId { get; private set; }
    public EntityRef(int refId, int hostId, string name)
    {
        //only want to call this for deserialization and from construct
        Name = name;
        RefId = refId;
        HostId = hostId;
    }

    public static EntityRef<TRef> Construct(TRef refer, Entity host, string name)
    {
        return new EntityRef<TRef>(refer.Id.Value, host.Id.Value, name);
    }
    public void Set(StrongWriteKey key, int newValue, IRepo repo, IServer server)
    {
        RefId = newValue;
        repo.RaiseValueChangedNotice(Name, HostId, key);
        if (key is HostWriteKey hKey)
        {
            var update = EntityVarUpdate.Encode<int>(Name, HostId, repo.Domain, newValue, hKey);
            ((HostServer)server).QueueUpdate(update);
        }
    }
    public bool TryGet(out TRef val, Action<int> invalidHandler = null)
    {
        val = (TRef) Game.I.Session.Data[RefId];
        var valid = val == null;
        if (valid == false) invalidHandler?.Invoke(RefId);
        return valid;
    }
}
