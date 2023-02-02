using Godot;
using System.Collections.Generic;

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
        List<Vector2> directionArr = new List<Vector2>();
        if (Input.IsActionPressed(InputMapping.UP))
        {
            directionArr.Add(Vector2.Up);
        }
        if (Input.IsActionPressed(InputMapping.DOWN))
        {
            directionArr.Add(Vector2.Down);
        }
        if (Input.IsActionPressed(InputMapping.RIGHT))
        {
            directionArr.Add(Vector2.Right);
        }
        if (Input.IsActionPressed(InputMapping.LEFT))
        {
            directionArr.Add(Vector2.Left);
        }
        if (directionArr.Count > 0)
        {
            baseBody.MoveTowards(directionArr);
        }
        else
        {
            baseBody.DeactivateAll();
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

    public void RemoveProgressPart()
    {
        baseBody.RemoveChild(progressPart);
        progressPart = null;
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
