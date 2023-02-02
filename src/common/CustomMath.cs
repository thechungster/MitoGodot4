using Godot;
using Godot.Collections;
using System.Collections.Generic;

public class CustomMath
{

    public static bool DoPartsIntersect(BasePart partOne, BasePart partTwo, float rotation)
    {
        Vector2[] polygonVectorOne = _createPolygonVector(partOne.GetPolygon(), partOne.GlobalPosition, partOne.Rotation);
        Vector2[] polygonVectorTwo = _createPolygonVector(partTwo.GetPolygon(), partTwo.GlobalPosition, partOne.Rotation + partTwo.Rotation);
        Array<Vector2[]> collisions = Geometry2D.IntersectPolygons(polygonVectorOne, polygonVectorTwo);
        GD.Print(partOne.Rotation, " : ", partTwo.Rotation);
        return collisions.Count > 0;
    }

    private static Vector2[] _createPolygonVector(Polygon2D polygon, Vector2 globalPos, float rotation)
    {
        Vector2[] polygonVectors = polygon.Polygon;
        Vector2[] result = new Vector2[polygonVectors.Length];
        for (int i = 0; i < polygonVectors.Length; i++)
        {
            result[i] = polygonVectors[i].Rotated(rotation) + globalPos;
        }
        return result;
    }
}