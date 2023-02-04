using Godot;
using System;

public partial class ShooterEnemy : Node2D
{
    private BaseEnemy _enemyPlayer;

    public override void _Ready()
    {
        _enemyPlayer = GetNode<BaseEnemy>("%BaseEnemy");
        // foreach (Node2D child in _enemyPlayer.GetChildren())
        // {
        //     if (!(child is BasePart))
        //     {
        //         return;
        //     }
        //     BasePart childPart = (BasePart)child;
        //     childPart.Player = _enemyPlayer;
        //     _enemyPlayer.AddProgressPart(childPart);
        //     NearestPointInfo info = new NearestPointInfo(child.GlobalPosition, new Line2D(), 0, childPart);
        //     _enemyPlayer.FinalizePart(info);
        // }
    }
}
