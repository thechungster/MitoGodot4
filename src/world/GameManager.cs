using Godot;
using System;

public partial class GameManager : Node2D
{
    private PartManager partManager;
    private BasePlayer basePlayer;
    public override void _Ready()
    {
        base._Ready();
        basePlayer = GetNode<BasePlayer>("%BasePlayer");
        partManager = GetNode<PartManager>("%PartManager");
        partManager.SetBasePlayer(basePlayer);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            // PackedScene thrusterScene = GD.Load<PackedScene>("res://src/parts/Thruster.tscn");
            // Thruster thruster = thrusterScene.Instantiate<Thruster>();
            // thruster.GlobalPosition = GetGlobalMousePosition();
            // AddChild(thruster);
        }
    }

}
