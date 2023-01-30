using Godot;
using System;

public partial class PartManager : Node2D
{
    public override void _Ready()
    {
        base._Ready();
    }

    protected void ButtonOnePressed()
    {
        GD.Print("Part one pressed");
    }
}
