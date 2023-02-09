using Godot;
using System;

public partial class DebugUi : Node2D
{
    private Label _mousePositionLabel;
    private Timer _timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _mousePositionLabel = this.GetNode<Label>("%MousePosition");
        _timer = GetNode<Timer>("%Timer");
        _timer.Timeout += UpdateMousePosLabel;
        _timer.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }

    private void UpdateMousePosLabel()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        String x = mousePos.x.ToString();
        String y = mousePos.y.ToString();
        this._mousePositionLabel.Text = x + ", " + y;
        _timer.Start();
    }
}
