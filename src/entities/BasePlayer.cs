using Godot;
using System.Collections.Generic;

public partial class BasePlayer : BaseEntity
{
    public override void _Process(double delta)
    {
        List<Vector2> directionArr = new List<Vector2>();
        if (Input.IsActionPressed(InputMapping.UP))
        {
            directionArr.Add(Vector2.Up);
        }
        if (Input.IsActionPressed(InputMapping.DOWN))
        {
            directionArr.Add(Vector2.Down);
        }
        if (Input.IsActionPressed(InputMapping.RIGHT))
        {
            directionArr.Add(Vector2.Right);
        }
        if (Input.IsActionPressed(InputMapping.LEFT))
        {
            directionArr.Add(Vector2.Left);
        }
        if (directionArr.Count > 0)
        {
            _baseBody.MoveTowards(directionArr);
        }
        else
        {
            _baseBody.DeactivateAll();
        }
    }
}
