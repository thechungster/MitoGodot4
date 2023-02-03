using Godot;
using System;

public partial class AttackBasePart : BasePart
{
    public override void _Ready()
    {
        base._Ready();
        partReady();
    }

    public virtual void Attack()
    {

    }
}
