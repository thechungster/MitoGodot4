using Godot;
using System;

public partial class MousePosition : Label
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        Vector2 mousePos = GetGlobalMousePosition();
        String x = mousePos.x.ToString();
        String y = mousePos.y.ToString();
        this.Text = x + ", " + y;
    }
}
