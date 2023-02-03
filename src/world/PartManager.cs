using Godot;
using System;


enum StepNumber
{
    UNSET,
    POSITION,
    ROTATION
}

public partial class PartManager : Node2D
{
    private BasePlayer player;
    private BasePart activePart = null;
    private NearestPointInfo nearestPointInfo = null;
    private StepNumber stepNumber = StepNumber.UNSET;
    private SavedPart _savedPart = null;
    private bool _isSaveable = false;

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
        if (activePart == null || stepNumber == StepNumber.UNSET)
        {
            return;
        }
        _isSaveable = !player.GetBaseBody().DoesPartCollide(activePart);
        Color color = _isSaveable ? CustomColors.IN_PROGRESS : CustomColors.INVALID;
        activePart.Modulate = color;
        if (stepNumber == StepNumber.POSITION)
        {
            _updateActivePartPosition();
        }
        // Always update Rotation
        if (stepNumber == StepNumber.ROTATION)
        {
            _updateFixedPartPosition();
        }
        _updateActivePartRotation();
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event.IsActionReleased(InputMapping.LEFT_MOUSE))
        {
            if (activePart == null || stepNumber == StepNumber.UNSET)
            {
                return;
            }
            if (stepNumber == StepNumber.POSITION)
            {
                stepNumber++;
            }
            else if (stepNumber == StepNumber.ROTATION && _isSaveable)
            {
                // finalize part
                player.FinalizePart(nearestPointInfo);
                activePart = null;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapping.ESCAPE) && activePart != null)
        {
            stepNumber--;
            if (stepNumber <= StepNumber.UNSET)
            {
                player.RemoveProgressPart();
                activePart = null;
            }
        }
    }

    protected void ButtonOnePressed()
    {
        _addPart("res://src/parts/Thruster.tscn");
    }

    protected void ButtonTwoPressed()
    {
        _addPart("res://src/parts/StaticShooter.tscn");
    }

    private void _addPart(String partPath)
    {
        if (activePart != null)
        {
            GD.PrintErr("Cannot add a part when one is already being placed.");
            return;
        }
        PackedScene partScene = GD.Load<PackedScene>(partPath);
        BasePart part = partScene.Instantiate<BasePart>();
        part.Player = player;
        _addProgressPart(part);
    }

    private void _addProgressPart(BasePart part)
    {
        // Can't create a part without finishing the previous one.
        if (activePart != null)
        {
            return;
        }
        stepNumber = StepNumber.POSITION;
        activePart = part;
        player.AddProgressPart(activePart);
    }

    /// <summary>
    /// Place part on closest point on the player body.
    /// </summary>
    private void _updateActivePartPosition()
    {
        nearestPointInfo = player.GetNearestPointOnBody(player.GetGlobalMousePosition());
        _savedPart = new SavedPart();
        _savedPart.GlobalPosition = nearestPointInfo.NearestPoint;
        _savedPart.PositionDifference = nearestPointInfo.NearestPoint - player.GetBaseBody().GlobalPosition;
        _savedPart.Rotation = player.GetBaseBody().Rotation;
        activePart.GlobalPosition = nearestPointInfo.NearestPoint; //nearestPointInfo.NearestPoint.Rotated(-1 * player.GetBaseBody().Rotation) - player.GetBaseBody().GlobalPosition;
    }

    /// <summary>
    /// Update the active part rotation based on the mouse position.
    /// </summary>
    private void _updateActivePartRotation()
    {
        Vector2 direction = activePart.GlobalPosition - activePart.GetGlobalMousePosition();
        float rotateRads = (float)Math.Atan2((double)direction.y, (double)direction.x) + (float)Math.PI / 2;
        activePart.Rotation = (float)(rotateRads) - player.GetBaseBody().Rotation;
    }

    /// <summary>
    /// Updates the position to be the same as the saved part position.
    /// </summary>
    private void _updateFixedPartPosition()
    {
        BaseBody baseBody = player.GetBaseBody();
        float normalizedRotation = baseBody.Rotation - _savedPart.Rotation;
        activePart.GlobalPosition = _savedPart.PositionDifference.Rotated(normalizedRotation) + baseBody.GlobalPosition;
    }

    private class SavedPart
    {
        // Saved global position of the nearest point.
        public Vector2 GlobalPosition;
        // Saved rotation of the base body
        public float Rotation;
        // Localized position of the nearest body WRT the base body global position.
        public Vector2 PositionDifference;
    }
}
