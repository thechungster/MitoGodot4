using Godot;
using System;

public partial class Thruster : BasePart
{
    private AnimatedSprite2D _thrusterFire;
    public override void _Ready()
    {
        _thrusterFire = GetNode<AnimatedSprite2D>("%ThrusterFire");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("thruster"))
        {
            _isActive = true;
        }
        if (Input.IsActionJustReleased("thruster"))
        {
            _isActive = false;
        }
        if (_isSet)
        {
            _thrusterFire.Visible = _isActive;
        }
    }


    private Vector2 thrust = new Vector2(0, -100);
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        PhysicsBody2D parent = (PhysicsBody2D)GetParent();
        Vector2 thrustDirection = thrust.Rotated(parent.Rotation + savedRotation);
        float rotateRads = (float)Math.Atan2((double)-1 * thrustDirection.y, (double)thrustDirection.x) + (float)(Math.PI / 2 * 3);

        DebugUtils.CreateSingleGlobalDebugArrow(this, Vector2.Zero, rotateRads);

        if (_isActive && _isSet)
        {
            Vector2 rotatedThrust = thrust.Rotated(parent.Rotation + savedRotation);
            ApplyForce(rotatedThrust);
        }
    }
}
