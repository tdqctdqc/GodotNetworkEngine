
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Game : Node
{
    public static Game I { get; private set; }
    public ISession Session { get; private set; }
    public override void _Ready()
    {
        if (I != null)
        {
            throw new Exception();
        }
        I = this;

        Serializer.Setup();
        
        var session = new HostSession();
        Session = session;
        session.Name = "Session";        
        AddChild(session);

    }
}

public class Doot
{
    public Vector2 V2 { get; private set; }

    public Doot()
    {
        V2 = Vector2.One;
    }

    public void DootF(Action<Vector2> act)
    {
        act(V2);
    }
}
