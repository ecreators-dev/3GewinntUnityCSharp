using System;
using UnityEngine;

public class CellInitParameters
{
    public ICellComponent ZelleCopy { get; set; }
    public IContentCreationConfig[] PoolData { get; set; }
    public Func<EContentType, (int column, int row)[]> FindCellWithContentTypeHandler { get; set; }
    public Action<IContentComponent> OnCellContentCreatedHandler { get; set; }
    public Vector3 CellPosition => ZelleCopy.transform.position;

    public (int column, int row)[] FindCellsWithContentType(EContentType contentType)
    {
        return this.FindCellWithContentTypeHandler?.Invoke(contentType) ?? new (int column, int row)[0];
    }

    public void TriggerContentCreatedInCell(IContentComponent prefabCopy)
    {
        this.OnCellContentCreatedHandler?.Invoke(prefabCopy);
    }
}