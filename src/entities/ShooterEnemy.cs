using Godot;
using System;
using System.Collections.Generic;

public partial class ShooterEnemy : Node2D
{
    private BaseEnemy _enemyPlayer;

    public override void _Ready()
    {
        _enemyPlayer = GetNode<BaseEnemy>("%BaseEnemy");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        List<Vector2> d = new List<Vector2>();
        d.Add(new Vector2(-1, 0));

        _enemyPlayer.GetBaseBody().MoveTowards(d);
    }
}
