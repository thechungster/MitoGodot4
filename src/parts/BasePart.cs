using Godot;
using System;

public partial class BasePart : RigidBody2D
{
    protected bool isActive = false;

    public override void _Ready()
    {

    }

    public virtual Vector2 GetNearestPoint(Vector2 point)
    {
        Polygon2D polygon = GetNode<Polygon2D>("%Polygon2D");
        if (polygon == null)
        {
            throw new NotImplementedException("Part has no polygon.");
        }
        return getNearestPointOnPolygon(polygon.Polygon, point);
    }

    protected Vector2 getNearestPointOnPolygon(Vector2[] polygon, Vector2 point)
    {
        float minDistanceSquared = float.MaxValue;
        Vector2 minDistancePoint = Vector2.Zero;
        for (int i = 0; i < polygon.Length - 1; i++)
        {
            Line2D line = new Line2D();

            line.AddPoint(polygon[i]);
            line.AddPoint(polygon[(i + 1) % polygon.Length]);

            Vector2 nearestPoint = getNearestPoint(line, point);
            float distanceSquared = point.DistanceSquaredTo(nearestPoint);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
                minDistancePoint = nearestPoint;
            }

        }
        return minDistancePoint;
    }

    private Vector2 getNearestPoint(Line2D line, Vector2 point)
    {
        Vector2 ab = line.Points[1] - line.Points[0];
        float distOnLine = ((point - line.Points[0]).Dot(ab)) / ab.Dot(ab);

        if (distOnLine < 0)
        {
            distOnLine = 0;
        }
        else if (distOnLine > 1)
        {
            distOnLine = 1;
        }

        return line.Points[0] + distOnLine * ab;
    }
}