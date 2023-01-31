using Godot;
using System;

public partial class DebugUi : Node2D
{
    private Label mousePositionLabel;
    private Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.mousePositionLabel = this.GetNode<Label>("%MousePosition");
        this.timer = GetNode<Timer>("%Timer");
        timer.Timeout += UpdateMousePosLabel;
        timer.Start();
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
        this.mousePositionLabel.Text = x + ", " + y;
        timer.Start();
    }
}
