using Godot;
using System;

public partial class Thruster : MovementBasePart
{
    private AnimatedSprite2D _thrusterFire;
    private Vector2 thrust = new Vector2(0, -100);

    public override void _Ready()
    {
        base._Ready();
        partReady();
        _thrusterFire = GetNode<AnimatedSprite2D>("%ThrusterFire");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("thruster"))
        {
            Activate();
        }
        if (Input.IsActionJustReleased("thruster"))
        {
            Deactivate();
        }
        if (_isSet)
        {
            _thrusterFire.Visible = _isActive;
        }
    }

    public override Vector2 GetMovementDirection()
    {
        PhysicsBody2D parent = (PhysicsBody2D)GetParent();
        Vector2 thrustDirection = thrust.Rotated(parent.Rotation + savedRotation);
        float rotateRads = (float)Math.Atan2((double)-1 * thrustDirection.y, (double)thrustDirection.x) + (float)(Math.PI / 2 * 3);

        return thrust.Rotated(parent.Rotation + savedRotation);
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        if (_isActive && _isSet)
        {
            ApplyForce(GetMovementDirection());
        }
    }
}
