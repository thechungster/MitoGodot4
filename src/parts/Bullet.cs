using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    public AttackBasePart BulletOwner;
    private Vector2 _projectileSpeed = new Vector2(0, 100);

    public override void _Process(double delta)
    {
        Velocity = _projectileSpeed.Rotated(GlobalRotation);
        MoveAndSlide();
    }

    private void OnBodyEntered(Node2D node)
    {
        if (node == BulletOwner)
        {
            return;
        }
        QueueFree();
    }
}
