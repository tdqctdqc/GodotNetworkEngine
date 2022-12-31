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
        if (isHost)
        {
            var server = new HostServer();
            var logic = new HostLogic();
            server.SetDependencies(logic);
            logic.SetDependencies(server);
            server.Name = "Server";
            AddChild(server);
        }
        else
        {
            var server = new ClientServer();
            server.Name = "Server";
            AddChild(server);
        }
        
        Data = new Data();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
