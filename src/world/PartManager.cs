using Godot;
using System;

public partial class PartManager : Node2D
{
    private BasePlayer player;
    public override void _Ready()
    {
        base._Ready();
    }

    public void SetBasePlayer(BasePlayer player)
    {
        this.player = player;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            Vector2 nearestPoint = player.GetNearestPointOnBody(player.GetLocalMousePosition());
            // DebugUtils utils = GetNode<DebugUtils>("/root/DebugUtils");
            // utils.CreateLocalPosDebugIcon(player, nearestPoint);
        }
    }

    protected void ButtonOnePressed()
    {
        GD.Print("Part one pressed");
    }
}
