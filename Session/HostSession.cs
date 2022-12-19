using Godot;
using System;

public class HostSession : Node, ISession
{
    public Data Data { get; private set; }
    public override void _Ready()
    {
        var server = new HostServer();
        server.Name = "Server";
        AddChild(server);
        var logic = new HostLogic();
        Data = new Data();
        server.SetDependencies(logic);
        logic.SetDependencies(server);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
