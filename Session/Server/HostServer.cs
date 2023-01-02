using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class HostServer : Node, IServer
{
    public int NetworkId => 1;

    public static HostServer ForTest;
    private HostLogic _logic;
    private List<IUpdate> _queuedUpdates;
    private List<string> _queuedUpdateTypes;
    private List<int> _clients;
    private NetworkedMultiplayerENet _network; 
    private string _ip = "127.0.0.1";
    private int _port = 3306;
    private int _maxPlayers = 100;
    public override void _Ready()
    {
        ForTest = this;
        _clients = new List<int>();
        _queuedUpdates = new List<IUpdate>();
        _queuedUpdateTypes = new List<string>();
        _network = new NetworkedMultiplayerENet();
        _network.CreateServer(_port, _maxPlayers);
        GetTree().NetworkPeer = _network;
        _network.Connect("peer_connected", this, nameof(PeerConnected));
        _network.Connect("peer_disconnected", this, nameof(PeerDisconnected));
    }
    
    public void SetDependencies(HostLogic logic)
    {
        _logic = logic;
    }

    public void QueueUpdate(IUpdate u)
    {
        _queuedUpdates.Add(u);
        _queuedUpdateTypes.Add(u.UpdateType);
        BroadcastUpdates();
    }

    private void BroadcastUpdates()
    {
        var updatesJson = System.Text.Json.JsonSerializer.Serialize(_queuedUpdates.Select(u => u.Serialize()));
        var updateTypesJson = System.Text.Json.JsonSerializer.Serialize(_queuedUpdateTypes);
        Rpc(nameof(ClientServer.ReceiveUpdates), updatesJson, updateTypesJson);
        _queuedUpdates.Clear();
        _queuedUpdateTypes.Clear();
    }
    private void PeerConnected(int id)
    {
        _clients.Add(id);
        GD.Print("peer " + id + " connected");
        RpcId(id, nameof(ClientServer.OnConnectionSucceeded));
        var stateTransfer = StateTransferUpdate.Encode(new HostWriteKey());
        var updateJsons = new List<string> {stateTransfer.Serialize()};
        var updateJsonsString = System.Text.Json.JsonSerializer.Serialize(updateJsons);
        var updateTypes = new List<string> {StateTransferUpdate.UpdateType};
        var updateTypesString = System.Text.Json.JsonSerializer.Serialize(updateTypes);

        RpcId(id, nameof(ClientServer.ReceiveUpdates), updateJsonsString, updateTypesString);
    }
    private void PeerDisconnected(int id)
    {
        _clients.Remove(id);
    }
}
