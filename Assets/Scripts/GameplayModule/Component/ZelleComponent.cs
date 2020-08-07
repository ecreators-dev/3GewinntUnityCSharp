using System;
using System.Collections;
using UnityEngine;

public class ZelleComponent : MonoBehaviour, ICellComponent
{
    public event Action<IContentComponent> ContentReachedCellEvent;

    public (int column, int row) Index { get; set; }

    public ZellInhaltComponent Inhalt
    {
        get
        {
            foreach (object child in this.transform)
            {
                if (child is GameObject go)
                {
                    return go.GetComponent<ZellInhaltComponent>();
                }
            }
            return null;
        }
    }

    public GameObject GameObject => gameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ZellInhaltComponent content))
        {
            content.Cell = this;
            StartCoroutine(WaitForStopMovement(content));
        }
    }

    private IEnumerator WaitForStopMovement(ZellInhaltComponent content)
    {
        yield return new WaitUntil(() => content.NotMoving);

        if (content.Cell == (ICellComponent)this)
        {
            ContentReachedCellEvent?.Invoke(content);
        }
    }

    public Component GetSelfComponent() => this;
}
