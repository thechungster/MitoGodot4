using Godot;
using System;

public partial class DebugUtils : Node
{
    public void CreateGlobalPosDebugIcon(Node parent, Vector2 position)
    {
        PackedScene scene = GD.Load<PackedScene>("res://src/ui/debug/DebugPoint.tscn");
        Node2D n = scene.Instantiate<Node2D>();
        n.GlobalPosition = position;
        parent.AddChild(n);
    }

    public void CreateLocalPosDebugIcon(Node parent, Vector2 position)
    {
        PackedScene scene = GD.Load<PackedScene>("res://src/ui/debug/DebugPoint.tscn");
        Node2D n = scene.Instantiate<Node2D>();
        n.Position = position;
        parent.AddChild(n);
    }
}
