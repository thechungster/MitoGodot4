using Godot;
using System;

public partial class StaticShooter : AttackBasePart
{

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("thruster"))
        {
            Attack();
        }
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void Attack()
    {
        PackedScene bulletScene = Preloader.Instance.GetResource<PackedScene>("res://src/parts/Bullet.tscn");
        Bullet bullet = bulletScene.Instantiate<Bullet>();
        bullet.GlobalPosition = GlobalPosition + new Vector2(0, 12).Rotated(GlobalRotation);
        bullet.GlobalRotation = GlobalRotation;
        Player.AddChild(bullet);
    }
}
