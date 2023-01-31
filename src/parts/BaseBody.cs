using Godot;
using System;

public partial class BaseBody : BasePart
{

    public override void _Ready()
    {
        base._Ready();
    }

    public override Vector2 GetNearestPoint(Vector2 point)
    {
        Polygon2D polygon = GetNode<Polygon2D>("%Polygon2D");
        return getNearestPointOnPolygon(polygon.Polygon, point);
    }
}
