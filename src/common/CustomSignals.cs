using Godot;
using System;

// Custom signal params must extend Godot.Object

public partial class CustomSignals : Node
{
    [Signal]
    public delegate void ExampleEventHandler(int i);
}
