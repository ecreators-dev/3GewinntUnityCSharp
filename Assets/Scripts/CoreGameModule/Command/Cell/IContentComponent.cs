using UnityEngine;

public interface IContentComponent
{
    ICellComponent Cell { get; set; }

    GameObject GameObject { get; }
    
    EContentType ContentType { get; }

    Component GetSelfComponent();
}