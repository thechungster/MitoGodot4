using Godot;
using System;

public partial class Thruster : RigidBody2D
{
    private Vector2 thrust = new Vector2(0, -100);
    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        base._IntegrateForces(state);
        ApplyForce(thrust.Rotated(Rotation));
    }
}
