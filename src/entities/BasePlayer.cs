using Godot;
using System;

public partial class BasePlayer : Node2D
{
    private BaseBody baseBody;
    public override void _Ready()
    {
        baseBody = GetNode<BaseBody>("%BaseBody");
        GD.Print(baseBody);
        // Thruster thruster = GetNode<Thruster>("%Thruster");
        // FixedJoint joint = new FixedJoint();
        // joint.ConnectBodies(baseBody, thruster);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustReleased(InputMapping.LEFT_MOUSE))
        {
        }
    }

    public NearestPointInfo GetNearestPointOnBody(Vector2 point)
    {
        return baseBody.GetNearestPoint(point);
    }

    public BaseBody GetBaseBody()
    {
        return baseBody;
    }

    public void AddPart(BasePart part)
    {
        baseBody.AddChild(part);
    }
}
