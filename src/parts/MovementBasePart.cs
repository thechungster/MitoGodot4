using Godot;
using System;

public partial class MovementBasePart : BasePart
{

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public virtual Vector2 GetMovementDirection()
    {
        throw new NotImplementedException("Inherited class should be implementing GetMovementDirection");
    }
}
