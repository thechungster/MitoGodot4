using Godot;

public partial class BaseEntity : Node2D
{

    protected BaseBody _baseBody;
    protected BasePart _progressPart = null;

    public override void _Ready()
    {
        _baseBody = GetNode<BaseBody>("%BaseBody");
    }

    public NearestPointInfo GetNearestPointOnBody(Vector2 point)
    {
        return _baseBody.GetNearestPoint(point, _baseBody);
    }

    public BaseBody GetBaseBody()
    {
        return _baseBody;
    }

    /// <summary>
    /// Part that is just in progress and temporarily added as a child. Will be removed once the part is either finalized or canceled.
    /// </summary>
    public void AddProgressPart(BasePart part)
    {
        _progressPart = part;
        _baseBody.AddChild(part);
    }

    public void RemoveProgressPart()
    {
        _baseBody.RemoveChild(_progressPart);
        _progressPart = null;
    }

    public void FinalizePart(NearestPointInfo nearestPointInfo)
    {
        if (_progressPart == null)
        {
            GD.PrintErr("Progress part is null");
        }
        FixedJoint joint = new FixedJoint();
        joint.ConnectBodies(_progressPart, nearestPointInfo.Part);
        // baseBody.RemoveChild(progressPart);
        nearestPointInfo.Part.AttachPart(_progressPart);
        _progressPart.FinishSet();
        _progressPart = null;
    }
}