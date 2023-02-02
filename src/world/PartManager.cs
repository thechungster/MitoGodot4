using Godot;
using System;

public partial class PartManager : Node2D
{
    private BasePlayer player;
    private BasePart activePart = null;
    private NearestPointInfo nearestPointInfo = null;

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
            nearestPointInfo = player.GetNearestPointOnBody(player.GetGlobalMousePosition());
            activePart.GlobalPosition = nearestPointInfo.NearestPoint; ;

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
            player.FinalizePart(nearestPointInfo);
            activePart = null;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapping.ESCAPE) && activePart != null)
        {
            player.RemoveProgressPart();
            activePart = null;
        }
    }

    protected void ButtonOnePressed()
    {
        GD.Print("Part one pressed");
        PackedScene thrusterScene = GD.Load<PackedScene>("res://src/parts/Thruster.tscn");
        Thruster thruster = thrusterScene.Instantiate<Thruster>();
        _addProgressPart(thruster);
    }

    private void _addProgressPart(BasePart part)
    {
        activePart = part;
        player.AddProgressPart(activePart);
    }
}
