using Godot;
using System;
using System.Collections.Generic;

public class HostServer : Node, IServer
{
    private HostLogic _logic;
    private List<IUpdate> _queuedUpdates;
    private List<string> _queuedUpdateTypes;
    public override void _Ready()
    {
        _queuedUpdates = new List<IUpdate>();
        _queuedUpdateTypes = new List<string>();
    }

    public void SetDependencies(HostLogic logic)
    {
        _logic = logic;
    }

    public void QueueUpdate(IUpdate u)
    {
        _queuedUpdates.Add(u);
        _queuedUpdateTypes.Add(u.UpdateType);
    }

    private void BroadcastUpdates()
    {
        
    }
}
