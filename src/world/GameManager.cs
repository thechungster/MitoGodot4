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
}
