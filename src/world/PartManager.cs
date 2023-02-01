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
            closestPoint = player.GetNearestPointOnBody(player.GetLocalMousePosition());
            activePart.Position = closestPoint;

            // Figure out the rotation pointing to the mouse
            Vector2 v = activePart.GlobalPosition - activePart.GetGlobalMousePosition();
            float rotateRads = (float)Math.Atan(v.y / v.x) + (float)Math.PI / 2;
            if (v.x <= 0)
            {
                rotateRads += (float)Math.PI;
            }
            activePart.Rotation = (float)(rotateRads % (2 * Math.PI));
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
            activePart = null;
        }
    }

    protected void ButtonOnePressed()
    {
        GD.Print("Part one pressed");
        PackedScene thrusterScene = GD.Load<PackedScene>("res://src/parts/Thruster.tscn");
        Thruster thruster = thrusterScene.Instantiate<Thruster>();
        activePart = thruster;
        player.AddPart(activePart);
    }
}
