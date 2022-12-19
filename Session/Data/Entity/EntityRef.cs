using Godot;
using System;

public class EntityRef<T> where T : Entity
{
    public int Id { get; private set; }
    public EntityRef(int id)
    {
        Id = id;
    }
    public T Get(out bool valid, Action invalidHandler = null)
    {
        var val = (T) Game.I.Session.Data[Id];
        valid = val == null;
        if (valid == false && invalidHandler != null) invalidHandler();
        return val;
    }
}
