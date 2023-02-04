using Godot;

public partial class BaseEnemy : BaseEntity
{
    public override void _Ready()
    {
        base._Ready();

        foreach (Node2D child in GetChildren())
        {
            if (!(child is BasePart))
            {
                return;
            }
            if (child is BaseBody)
            {
                continue;
            }
            BasePart childPart = (BasePart)child;
            childPart.Player = this;
            RemoveChild(childPart);
            this.AddProgressPart(childPart);
            NearestPointInfo info = new NearestPointInfo(child.GlobalPosition, new Line2D(), 0, childPart);
            this.FinalizePart(info);
        }
    }
}