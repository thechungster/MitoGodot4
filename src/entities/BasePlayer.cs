using Godot;
using System;

public partial class BasePlayer : Node2D
{
    private BaseBody baseBody;
    private BasePart progressPart = null;
    public override void _Ready()
    {
        baseBody = GetNode<BaseBody>("%BaseBody");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustReleased(InputMapping.LEFT_MOUSE))
        {
        }
    }

    public NearestPointInfo GetNearestPointOnBody(Vector2 point)
    {
        return baseBody.GetNearestPoint(point, baseBody);
    }

    public BaseBody GetBaseBody()
    {
        return baseBody;
    }

    /// <summary>
    /// Part that is just in progress and temporarily added as a child. Will be removed once the part is either finalized or canceled.
    /// </summary>
    public void AddProgressPart(BasePart part)
    {
        progressPart = part;
        baseBody.AddChild(part);
    }

    public void FinalizePart(NearestPointInfo nearestPointInfo)
    {
        if (progressPart == null)
        {
            GD.PrintErr("Progress part is null");
        }
        FixedJoint joint = new FixedJoint();
        joint.ConnectBodies(progressPart, nearestPointInfo.Part);
        // baseBody.RemoveChild(progressPart);
        nearestPointInfo.Part.AttachPart(progressPart);
        progressPart.FinishSet();
        progressPart = null;
    }
}
