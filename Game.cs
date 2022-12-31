
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
        
        var session = new Session();
        Session = session;
        session.Name = "Session";        
        AddChild(session);
        session.Start(true);
    }
}
