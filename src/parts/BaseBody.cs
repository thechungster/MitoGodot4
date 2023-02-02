using Godot;
using System;

public partial class BaseBody : BasePart
{
    public override void _Ready()
    {
        base._Ready();
        Rotation = (float)(Math.PI / 3);
    }
}
