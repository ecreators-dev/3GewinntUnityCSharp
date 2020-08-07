using System;
using UnityEngine;

public interface IDragDropComponent
{
    event Action DragStartEvent;
    event Action DragMoveEvent;
    event Action DragEndEvent;
    event Action DestroyEvent;

    /// <summary>
    /// Just call <see cref="DragStartEvent"/> like e.g. <code>public void OnMouseDown() => <see cref="DragStartEvent"/>?.Invoke();</code>
    /// </summary>
    void OnMouseDown();
    /// <summary>
    /// Just call <see cref="DragMoveEvent"/> like e.g. <code>public void OnMouseDrag() => <see cref="DragMoveEvent" />?.Invoke();</code>
    /// </summary>
    void OnMouseDrag();
    /// <summary>
    /// Just call <see cref="DragEndEvent"/> like e.g. <code>public void OnMouseUp() => <see cref="DragEndEvent"/>?.Invoke();</code>
    /// </summary>
    void OnMouseUp();
    Vector3 GetWorldPosition();
    void SetWorldPosition(Vector3 objectPosition);
}