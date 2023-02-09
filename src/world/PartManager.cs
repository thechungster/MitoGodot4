using Godot;
using System;


enum StepNumber
{
    Unset,
    Position,
    Rotation
}

public partial class PartManager : Node2D
{
    private BasePlayer _player;
    private BasePart _activePart = null;
    private NearestPointInfo _nearestPointInfo = null;
    private StepNumber _stepNumber = StepNumber.Unset;
    private SavedPart _savedPart = null;
    private bool _isSaveable = false;

    public override void _Ready()
    {
        base._Ready();
    }

    public void SetBasePlayer(BasePlayer basePlayer)
    {
        _player = basePlayer;
    }

    public override void _Process(double delta)
    {
        if (_activePart == null || _stepNumber == StepNumber.Unset)
        {
            return;
        }
        _isSaveable = !_player.GetBaseBody().DoesPartCollide(_activePart);
        Color color = _isSaveable ? CustomColors.IN_PROGRESS : CustomColors.INVALID;
        _activePart.Modulate = color;
        if (_stepNumber == StepNumber.Position)
        {
            _updateActivePartPosition();
        }
        // Always update Rotation
        if (_stepNumber == StepNumber.Rotation)
        {
            _updateFixedPartPosition();
        }
        _updateActivePartRotation();
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event.IsActionReleased(InputMapping.LEFT_MOUSE))
        {
            if (_activePart == null || _stepNumber == StepNumber.Unset)
            {
                return;
            }
            if (_stepNumber == StepNumber.Position)
            {
                _stepNumber++;
            }
            else if (_stepNumber == StepNumber.Rotation && _isSaveable)
            {
                // finalize part
                _player.FinalizePart(_nearestPointInfo);
                _activePart = null;
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed(InputMapping.ESCAPE) && _activePart != null)
        {
            _stepNumber--;
            if (_stepNumber <= StepNumber.Unset)
            {
                _player.RemoveProgressPart();
                _activePart = null;
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
        if (_activePart != null)
        {
            GD.PrintErr("Cannot add a part when one is already being placed.");
            return;
        }
        PackedScene partScene = GD.Load<PackedScene>(partPath);
        BasePart part = partScene.Instantiate<BasePart>();
        part.Player = _player;
        _addProgressPart(part);
    }

    private void _addProgressPart(BasePart part)
    {
        // Can't create a part without finishing the previous one.
        if (_activePart != null)
        {
            return;
        }
        _stepNumber = StepNumber.Position;
        _activePart = part;
        _player.AddProgressPart(_activePart);
    }

    /// <summary>
    /// Place part on closest point on the player body.
    /// </summary>
    private void _updateActivePartPosition()
    {
        _nearestPointInfo = _player.GetNearestPointOnBody(_player.GetGlobalMousePosition());
        _savedPart = new SavedPart();
        _savedPart.GlobalPosition = _nearestPointInfo.NearestPoint;
        _savedPart.PositionDifference = _nearestPointInfo.NearestPoint - _player.GetBaseBody().GlobalPosition;
        _savedPart.Rotation = _player.GetBaseBody().Rotation;
        _activePart.GlobalPosition = _nearestPointInfo.NearestPoint; //nearestPointInfo.NearestPoint.Rotated(-1 * player.GetBaseBody().Rotation) - player.GetBaseBody().GlobalPosition;
    }

    /// <summary>
    /// Update the active part rotation based on the mouse position.
    /// </summary>
    private void _updateActivePartRotation()
    {
        Vector2 direction = _activePart.GlobalPosition - _activePart.GetGlobalMousePosition();
        float rotateRads = (float)Math.Atan2((double)direction.y, (double)direction.x) + (float)Math.PI / 2;
        _activePart.Rotation = (float)(rotateRads) - _player.GetBaseBody().Rotation;
    }

    /// <summary>
    /// Updates the position to be the same as the saved part position.
    /// </summary>
    private void _updateFixedPartPosition()
    {
        BaseBody baseBody = _player.GetBaseBody();
        float normalizedRotation = baseBody.Rotation - _savedPart.Rotation;
        _activePart.GlobalPosition = _savedPart.PositionDifference.Rotated(normalizedRotation) + baseBody.GlobalPosition;
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
