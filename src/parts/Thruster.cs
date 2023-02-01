using Godot;
using System;

public partial class Thruster : BasePart
{
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("thruster"))
        {
            isActive = true;
        }
        if (Input.IsActionJustReleased("thruster"))
        {
            isActive = false;
        }
    }


    private Vector2 thrust = new Vector2(0, -100);
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        PhysicsBody2D parent = (PhysicsBody2D)GetParent();
        Vector2 thrustDirection = thrust.Rotated(parent.Rotation + savedRotation);
        float rotateRads = (float)Math.Atan2((double)-1 * thrustDirection.y, (double)thrustDirection.x) + (float)(Math.PI / 2 * 3);

        DebugUtils.CreateSingleGlobalDebugArrow(this, Vector2.Zero, rotateRads);

        if (isActive && isSet)
        {
            Vector2 rotatedThrust = thrust.Rotated(parent.Rotation + savedRotation);
            ApplyForce(rotatedThrust);
        }
    }
}
