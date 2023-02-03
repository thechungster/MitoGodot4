using Godot;
using System;

public partial class Bullet : CharacterBody2D
{
    private Vector2 _projectileSpeed = new Vector2(0, 100);

    public override void _Process(double delta)
    {
        Velocity = _projectileSpeed.Rotated(GlobalRotation);
        MoveAndSlide();
    }
}
