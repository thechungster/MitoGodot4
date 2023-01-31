using Godot;
using System;

public partial class BasePlayer : Node2D
{
    private BaseBody baseBody;
    public override void _Ready()
    {
        base._Ready();

        baseBody = GetNode<BaseBody>("%BaseBody");
        Thruster thruster = GetNode<Thruster>("%Thruster");
        // FixedJoint joint = new FixedJoint();
        // joint.ConnectBodies(baseBody, thruster);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustReleased(InputMapping.LEFT_MOUSE))
        {
        }
    }

    public Vector2 GetNearestPointOnBody(Vector2 point)
    {
        return baseBody.GetNearestPoint(point);
    }
}
