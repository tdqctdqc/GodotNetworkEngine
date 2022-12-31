using Godot;
using System;

public class StartScene : Node
{
    private Label _label;
    private Button _hostBtn, _clientBtn, _addEntityBtn;
    public override void _Ready()
    {
        _hostBtn = (Button)FindNode("Host");
        _hostBtn.Connect("button_up", this, nameof(StartAsHost));
        _clientBtn = (Button)FindNode("Client");
        _clientBtn.Connect("button_up", this, nameof(StartAsClient));
        _addEntityBtn = (Button)FindNode("AddEntity");
        _addEntityBtn.Connect("button_up", this, nameof(AddEntity));
        _addEntityBtn.Visible = false;
        _label = (Label)FindNode("Label");
    }

    public void StartAsHost()
    {
        GD.Print("starting as host");
        Game.I.StartSession(true);
        _clientBtn.QueueFree();
        _hostBtn.QueueFree();
        _label.Text = "Host";
        _addEntityBtn.Visible = true;
    }

    public void StartAsClient()
    {
        GD.Print("starting as client");
        Game.I.StartSession(false);
        _clientBtn.QueueFree();
        _hostBtn.QueueFree();
        _label.Text = "Client";

    }

    public void AddEntity()
    {
        Game.I.AddEntity();
    }
}
