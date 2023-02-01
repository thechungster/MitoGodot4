using Godot;
using System;

public partial class DebugUtils : Node
{
    public Node prevParent;
    public Node2D arrow;

    public static DebugUtils GetInstance(Node node)
    {
        return node.GetNode<DebugUtils>("/root/DebugUtils");
    }

    public static void CreateLocalPosDebugIcon(Node parent, Vector2 position)
    {
        DebugUtils.GetInstance(parent)._createLocalPosDebugIcon(parent, position);
    }

    public static void CreateGlobalPosDebugIcon(Node parent, Vector2 position)
    {
        DebugUtils.GetInstance(parent)._createGlobalPosDebugIcon(parent, position);
    }

    public static void CreateSingleGlobalDebugArrow(Node parent, Vector2 position, float rotation)
    {
        DebugUtils.GetInstance(parent)._createSingleGlobalDebugArrow(parent, position, rotation);
    }

    private void _createGlobalPosDebugIcon(Node parent, Vector2 position)
    {
        PackedScene scene = GD.Load<PackedScene>("res://src/ui/debug/DebugPoint.tscn");
        Node2D n = scene.Instantiate<Node2D>();
        n.GlobalPosition = position;
        parent.AddChild(n);
    }

    private void _createLocalPosDebugIcon(Node parent, Vector2 position)
    {
        PackedScene scene = GD.Load<PackedScene>("res://src/ui/debug/DebugPoint.tscn");
        Node2D n = scene.Instantiate<Node2D>();
        n.Position = position;
        parent.AddChild(n);
    }

    private void _createSingleGlobalDebugArrow(Node parent, Vector2 position, float rotation)
    {
        if (arrow != null)
        {
            prevParent.RemoveChild(arrow);
        }
        PackedScene scene = GD.Load<PackedScene>("res://src/ui/debug/DebugArrow.tscn");
        arrow = scene.Instantiate<Node2D>();
        prevParent = parent;
        arrow.GlobalPosition = position;
        arrow.Rotate(rotation);
        parent.AddChild(arrow);
    }
}
