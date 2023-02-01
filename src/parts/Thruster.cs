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
        if (isActive)
        {
            // base._IntegrateForces(state);
            ApplyForce(thrust.Rotated(Rotation));
        }
    }
}
