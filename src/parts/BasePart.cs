using Godot;
using System;
using System.Collections.Generic;

public partial class BasePart : RigidBody2D
{
    protected bool _isActive = false;
    protected bool _isSet = false;
    protected List<BasePart> attachedParts = new List<BasePart>();
    // For some reason the Rotation from _IntegrateForces is different from the actual Rotation, so we save it here.
    protected float savedRotation;


    public void FinishSet()
    {
        _isSet = true;
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("%CollisionShape2D");
        collisionShape.Disabled = false;
        savedRotation = Rotation;
    }

    public void MoveTowards(List<Vector2> directions)
    {
        if (this is MovementBasePart)
        {
            bool shouldDeactivate = true;
            foreach (Vector2 direction in directions)
            {
                MovementBasePart currentPart = (MovementBasePart)this;
                Vector2 movementDirection = currentPart.GetMovementDirection();
                Vector2 normalizedMovementDirection = movementDirection.Normalized();
                // 1 is similar, -1 is opposite.
                float similiarity = direction.Normalized().Dot(normalizedMovementDirection);
                if (similiarity > 0)
                {
                    shouldDeactivate = false;
                    currentPart.Activate();
                }
                if (shouldDeactivate)
                {
                    currentPart.Deactivate();
                }
            }
        }
        foreach (BasePart attachedPart in attachedParts)
        {
            attachedPart.MoveTowards(directions);
        }

    }

    public void DeactivateAll()
    {
        if (this is MovementBasePart)
        {
            MovementBasePart currentPart = (MovementBasePart)this;
            currentPart.Deactivate();
        }

        foreach (BasePart attachedPart in attachedParts)
        {
            attachedPart.DeactivateAll();
        }
    }


    /// <summary>
    /// Attach a part to the current part.
    /// </summary>
    public void AttachPart(BasePart part)
    {
        attachedParts.Add(part);
        // AddChild(part);
    }

    public virtual NearestPointInfo GetNearestPoint(Vector2 point, BaseBody baseBody)
    {
        Polygon2D polygon = GetNode<Polygon2D>("%Polygon2D");
        if (polygon == null)
        {
            throw new NotImplementedException("Part has no polygon.");
        }
        NearestPointInfo nearestPointInfo = new NearestPointInfo(Vector2.Zero, new Line2D(), float.MaxValue, this); ;
        foreach (BasePart attachedPart in attachedParts)
        {
            NearestPointInfo potentialNearestPoint = attachedPart.GetNearestPoint(point, baseBody);
            if (potentialNearestPoint.NearestDistanceSquared < nearestPointInfo.NearestDistanceSquared)
            {
                nearestPointInfo = potentialNearestPoint;
            }
        }

        Vector2[] vertices = polygon.Polygon;
        float minDistanceSquared = float.MaxValue;
        Vector2 minDistancePoint = Vector2.Zero;
        Line2D minDistanceLine = new Line2D();
        for (int i = 0; i < vertices.Length; i++)
        {
            // DebugUtils.CreateGlobalPosDebugIcon(this, vertices[i]);
            Line2D line = new Line2D();
            float rotation = this == baseBody ? Rotation : Rotation + baseBody.Rotation;
            line.AddPoint(vertices[i].Rotated(rotation) + GlobalPosition);
            line.AddPoint(vertices[(i + 1) % vertices.Length].Rotated(rotation) + GlobalPosition);

            Vector2 nearestPoint = getNearestPoint(line, point);
            float distanceSquared = point.DistanceSquaredTo(nearestPoint);

            if (distanceSquared < minDistanceSquared)
            {
                minDistanceSquared = distanceSquared;
                minDistancePoint = nearestPoint;
                minDistanceLine = line;
            }
        }
        NearestPointInfo potentialPoint = new NearestPointInfo(minDistancePoint, minDistanceLine, minDistanceSquared, this);
        if (potentialPoint.NearestDistanceSquared < nearestPointInfo.NearestDistanceSquared)
        {
            nearestPointInfo = potentialPoint;
        }
        return nearestPointInfo;
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
    public BasePart Part;

    public NearestPointInfo(Vector2 p, Line2D l, float d, BasePart part)
    {
        NearestPoint = p;
        NearestLine = l;
        NearestDistanceSquared = d;
        Part = part;
    }
}