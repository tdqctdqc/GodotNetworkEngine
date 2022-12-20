
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

        var meta = Serializer.GetEntityMeta<Player>();
        var player = new Player(1, "doot", new HostWriteKey());
        var playerJson = Serializer.GetEntityMeta<Player>().Serialize(player);
        // GD.Print(playerJson);
        var player2 = meta.Deserialize(playerJson);
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
