using Godot;
using System;

// https://gist.github.com/jotson/f31e4ac21217f8943870a18776e2b7d0
public partial class FixedJoint : PinJoint2D
{
    // The rotation to fix this joint at
    private float rotationFix = 0;
    private Vector2 positionFix = Vector2.Zero;

    /// <summary>
    /// Connect a fixed joint between two bodies. The first body should already exist, the second body should be the new part being added.
    /// </summary>
    public void ConnectBodies(PhysicsBody2D partAdded, PhysicsBody2D existingPart)
    {
        DisableCollision = true;
        partAdded.AddChild(this);
        NodeA = partAdded.GetPath();
        NodeB = existingPart.GetPath();

        positionFix = partAdded.Position;
        rotationFix = partAdded.Rotation;
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
        parentBody.SetDeferred("rotation", rotationFix);
        parentBody.SetDeferred("position", positionFix);
    }
}