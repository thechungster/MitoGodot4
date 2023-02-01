using Godot;
using System;

public partial class BasePart : RigidBody2D
{
    protected bool isActive = false;
    protected bool isSet = false;

    public override void _Ready()
    {

    }

    public void FinishSet()
    {
        isSet = true;
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("%CollisionShape2D");
        collisionShape.Disabled = false;
    }

    public virtual NearestPointInfo GetNearestPoint(Vector2 point)
    {
        Polygon2D polygon = GetNode<Polygon2D>("%Polygon2D");
        if (polygon == null)
        {
            throw new NotImplementedException("Part has no polygon.");
        }

        Vector2[] vertices = polygon.Polygon;
        float minDistanceSquared = float.MaxValue;
        Vector2 minDistancePoint = Vector2.Zero;
        Line2D minDistanceLine = new Line2D();
        for (int i = 0; i < vertices.Length; i++)
        {
            // DebugUtils.CreateGlobalPosDebugIcon(this, vertices[i]);
            Line2D line = new Line2D();

            line.AddPoint(vertices[i].Rotated(Rotation) + GlobalPosition);
            line.AddPoint(vertices[(i + 1) % vertices.Length].Rotated(Rotation) + GlobalPosition);

            Vector2 nearestPoint = getNearestPoint(line, point);
            float distanceSquared = point.DistanceSquaredTo(nearestPoint);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
                minDistancePoint = nearestPoint;
                minDistanceLine = line;
            }

        }
        return new NearestPointInfo(minDistancePoint, minDistanceLine, minDistanceSquared);
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

public class NearestPointInfo
{
    public Vector2 NearestPoint;
    public Line2D NearestLine;
    public float NearestDistanceSquared;

    public NearestPointInfo(Vector2 p, Line2D l, float d)
    {
        NearestPoint = p;
        NearestLine = l;
        NearestDistanceSquared = d;
    }
}