
using Godot;
using System;
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

        
        var player = new Player(1, "doot", new HostWriteKey());
        var playerJson = Serializer.Serialize(player);
        var player2 = Serializer.Deserialize<Player>(playerJson);
        GD.Print(player2.Name.Value);
    }
}

public class Doot
{
    public int Int { get; set; }

    public Doot(int i)
    {
        Int = i;
    }
}
