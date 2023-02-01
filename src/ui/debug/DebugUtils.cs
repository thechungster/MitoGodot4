using Godot;
using System;

public partial class DebugUtils : Node
{

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

    public void _createGlobalPosDebugIcon(Node parent, Vector2 position)
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
}
