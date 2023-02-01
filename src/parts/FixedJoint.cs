using Godot;
using System;

// https://gist.github.com/jotson/f31e4ac21217f8943870a18776e2b7d0
public partial class FixedJoint : PinJoint2D
{
    // The rotation to fix this joint at
    private float rotationFix = 0;

    /// <summary>
    /// Connect a fixed joint between two bodies. The first body should already exist, the second body should be the new part being added.
    /// </summary>
    public void ConnectBodies(PhysicsBody2D bodyA, PhysicsBody2D bodyB)
    {
        DisableCollision = true;
        bodyA.AddChild(this);
        NodeA = bodyA.GetPath();
        NodeB = bodyB.GetPath();

        float angleToBody = (GlobalPosition - bodyB.GlobalPosition).Angle();
        PhysicsBody2D parentBody = (PhysicsBody2D)GetParent();
        rotationFix = parentBody.GlobalRotation - angleToBody;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (NodeB == null)
        {
            GD.PrintErr("No nodeB");
        }
        PhysicsBody2D bodyB = GetNode<PhysicsBody2D>(NodeB);
        if (bodyB == null)
        {
            GD.PrintErr("NodeB cannot be found");
        }
        PhysicsBody2D parentBody = (PhysicsBody2D)GetParent();
        GD.Print(GlobalPosition);
        float angleToBody = (GlobalPosition - bodyB.GlobalPosition).Angle();
        parentBody.SetDeferred("rotation", angleToBody + rotationFix);
    }
}