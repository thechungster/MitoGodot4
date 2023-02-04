using Godot;
using System;
using System.Collections.Generic;

public partial class BasePart : RigidBody2D
{
    [Export]
    public BaseEntity Player;
    protected bool _isActive = false;
    protected bool _isSet = false;
    protected List<BasePart> attachedParts = new List<BasePart>();
    private Sprite2D _sprite;
    private Polygon2D _polygon2D;
    // For some reason the Rotation from _IntegrateForces is different from the actual Rotation, so we save it here.
    protected float savedRotation;

    public override void _Ready()
    {
        if (Player == null)
        {
            GD.PrintErr("Base player should be set on the part before instantiating.");
        }
        _polygon2D = GetNode<Polygon2D>("%Polygon2D");
    }

    /// <summary>
    /// Should be called by all parts in the ready function that can be placed.
    /// </summary>
    protected void partReady()
    {
        _sprite = GetNode<Sprite2D>("%Sprite2D");
        _sprite.SelfModulate = CustomColors.IN_PROGRESS;
    }

    public void FinishSet()
    {
        GD.Print(Player);
        CollisionPolygon2D collisionShape = GetNode<CollisionPolygon2D>("%CollisionPolygon2D");
        collisionShape.Disabled = false;
        savedRotation = Rotation;
        _sprite.SelfModulate = CustomColors.FINAL;
        _isSet = true;
    }

    public void SelfModulateSprite(Color c)
    {
        _sprite.SelfModulate = c;
    }

    public bool DoesPartCollide(BasePart attachingPart)
    {
        if (CustomMath.DoPartsIntersect(this, attachingPart))
        {
            return true;
        }

        foreach (BasePart attachedPart in attachedParts)
        {
            if (attachedPart.DoesPartCollide(attachingPart))
            {
                return true;
            }
        }
        return false;
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
    }

    public virtual NearestPointInfo GetNearestPoint(Vector2 point, BaseBody baseBody)
    {
        if (_polygon2D == null)
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

        Vector2[] vertices = _polygon2D.Polygon;
        float minDistanceSquared = float.MaxValue;
        Vector2 minDistancePoint = Vector2.Zero;
        Line2D minDistanceLine = new Line2D();
        for (int i = 0; i < vertices.Length; i++)
        {
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

    public Polygon2D GetPolygon()
    {
        return _polygon2D;
    }

    // public Vector2[] GetAdjustedPolygon() {
    // return _polygon2D.P
    // }

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

/// <summary>
/// Information of the nearest point. Nearest point is the GLobalPosition
/// </summary>
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