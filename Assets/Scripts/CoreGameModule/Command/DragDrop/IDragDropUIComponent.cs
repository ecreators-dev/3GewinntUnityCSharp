using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IDragDropUIComponent : IBeginDragHandler, IEndDragHandler, IDragHandler
{
    event Action<PointerEventData> DragStartEvent;
    event Action<PointerEventData> DragMoveEvent;
    event Action<PointerEventData> DragEndEvent;
    event Action DestroyEvent;

    void SetWorldPosition(Vector3 objectPosition);
}
