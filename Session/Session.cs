using Godot;
using System;

public class Session : Node, ISession
{
    public Data Data { get; private set; }
    public override void _Ready()
    {
        
    }

    public void Start(bool isHost)
    {
        IServer server;
        if (isHost)
        {
            var hServer = new HostServer();
            server = hServer;
            var logic = new HostLogic();
            hServer.SetDependencies(logic);
            logic.SetDependencies(hServer);
            hServer.Name = "Server";
            AddChild(hServer);
        }
        else
        {
            var cServer = new ClientServer();
            server = cServer;
            cServer.Name = "Server";
            AddChild(cServer);
        }
        
        Data = new Data(server);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
