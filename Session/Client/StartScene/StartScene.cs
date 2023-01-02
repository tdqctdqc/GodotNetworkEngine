using Godot;
using System;

public class StartScene : Node
{
    private Label _label;
    private Button _hostBtn, _clientBtn, _addEntityBtn, 
        _entityOverviewBtn, _changeEntityBtn, _triggerProcBtn;
    private EntityOverview _entityOverview;
    public override void _Ready()
    {
        _hostBtn = (Button)FindNode("Host");
        _hostBtn.Connect("button_up", this, nameof(StartAsHost));
        _clientBtn = (Button)FindNode("Client");
        _clientBtn.Connect("button_up", this, nameof(StartAsClient));
        _addEntityBtn = (Button)FindNode("AddEntity");
        _addEntityBtn.Connect("button_up", this, nameof(AddEntity));
        _addEntityBtn.Visible = false;
        
        _changeEntityBtn = (Button)FindNode("ChangeEntity");
        _changeEntityBtn.Connect("button_up", this, nameof(ChangeEntity));
        _changeEntityBtn.Visible = false;
        
        _entityOverview = (EntityOverview)FindNode("EntityOverview");
        
        _entityOverviewBtn = (Button)FindNode("EntityOverviewButton");
        _entityOverviewBtn.Connect("button_up", this, nameof(ShowEntityOverview));
        _entityOverviewBtn.Visible = false;

        _triggerProcBtn = (Button) FindNode("TriggerProcedure");
        _triggerProcBtn.Connect("button_up", this, nameof(TriggerProcedure));
        _triggerProcBtn.Visible = false;
        
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
        _entityOverviewBtn.Visible = true;
        _changeEntityBtn.Visible = true;
        _triggerProcBtn.Visible = true;
    }

    public void StartAsClient()
    {
        GD.Print("starting as client");
        Game.I.StartSession(false);
        _clientBtn.QueueFree();
        _hostBtn.QueueFree();
        _label.Text = "Client";
        _entityOverviewBtn.Visible = true;

    }

    public void AddEntity()
    {
        Game.I.AddEntity();
    }

    public void ShowEntityOverview()
    {
        _entityOverview.Popup_();
    }

    public void ChangeEntity()
    {
        Game.I.ChangeEntity();
    }

    public void TriggerProcedure()
    {
        Game.I.TriggerProcedure();
    }
}
