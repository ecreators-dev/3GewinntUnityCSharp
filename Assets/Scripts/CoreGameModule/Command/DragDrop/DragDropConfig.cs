using System;
using UnityEngine;
using UnityEngine.UIElements;

public class DragDropConfig
{
    public static readonly Func<bool> ALLOW_DRAG_ALWAYS = () => true;
    public static readonly Func<float> Z_IS_ZERO = () => 0f;

    /// <summary>
    /// Position where to place your 3d object
    /// </summary>
    public Func<float> GetZ { get; set; } = Z_IS_ZERO;

    /// <summary>
    /// Called if MouseButton is correct. Checks with this to start or not to start a drag process.
    /// </summary>
    public Func<bool> CheckDragStartAllowanceRule { get; set; } = ALLOW_DRAG_ALWAYS;

    /// <summary>
    /// Button on react to
    /// </summary>
    public MouseButton ClickButton { get; set; } = MouseButton.LeftMouse;
    
    /// <summary>
    /// Optional Position Correction. for in case of snapping during drag
    /// </summary>
    public DragPositionUpdateHandler DragPositionUpdateHandler { get; set; }
    
    /// <summary>
    /// Is called at end
    /// </summary>
    public Action DropHandler { get; set; }
}

public delegate Vector3 DragPositionUpdateHandler(Vector3 objectDragPositionByMouse, out bool cancelDrag);