using System;
using UnityEngine;

public interface ICellComponent
{
    event Action<IContentComponent> ContentReachedCellEvent;

    Transform transform { get; }
    
    (int column, int row) Index { get; set; }
    GameObject GameObject { get; }

    Component GetSelfComponent();
    T GetComponentInChildren<T>();
}