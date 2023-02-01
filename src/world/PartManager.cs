using Godot;
using System;

public partial class PartManager : Node2D
{
    private BasePlayer player;
    private BasePart activePart = null;
    private Vector2 closestPoint = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
    }

    public void SetBasePlayer(BasePlayer player)
    {
        this.player = player;
    }

    public override void _Process(double delta)
    {
        if (activePart != null)
        {
            // Place part on closest point on the player body
            NearestPointInfo nearestPointInfo = player.GetNearestPointOnBody(player.GetGlobalMousePosition());
            closestPoint = nearestPointInfo.NearestPoint;
            activePart.GlobalPosition = closestPoint;

            // Figure out the rotation pointing to the mouse
            Vector2 direction = activePart.GlobalPosition - activePart.GetGlobalMousePosition();
            float rotateRads = (float)Math.Atan2((double)direction.y, (double)direction.x) + (float)Math.PI / 2;
            activePart.Rotation = (float)(rotateRads) - player.GetBaseBody().Rotation;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton)
        {
            if (activePart == null)
            {
                return;
            }
            // finalize part    
            FixedJoint joint = new FixedJoint();
            joint.ConnectBodies(activePart, player.GetBaseBody());
            activePart.FinishSet();
            activePart = null;
        }
    }

    protected void ButtonOnePressed()
    {
        GD.Print("Part one pressed");
        PackedScene thrusterScene = GD.Load<PackedScene>("res://src/parts/Thruster.tscn");
        Thruster thruster = thrusterScene.Instantiate<Thruster>();
        activePart = thruster;
        // AddChild(activePart);
        player.AddPart(activePart);
    }
}
