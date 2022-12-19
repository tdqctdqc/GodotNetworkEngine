using Godot;
using System;
using System.Collections.Generic;

public class HostServer : Node, IServer
{
    private HostLogic _logic;
    private List<IUpdate> _queuedUpdates;
    public override void _Ready()
    {
        
    }

    public void SetDependencies(HostLogic logic)
    {
        _logic = logic;
    }

    public void QueueUpdate(IUpdate u)
    {
        _queuedUpdates.Add(u);
    }
}
