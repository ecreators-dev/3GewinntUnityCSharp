using System;
using UnityEngine;

public class BoardParameters
{
    public int Spalten { get; set; }
    public int Zeilen { get; set; }
    public float Abstand { get; set; }
    public ICellComponent ZelleVorlage { get; set; }
    public Transform TransformParent { get; set; }

    public Action<ICellComponent> OnCellCreatedHandler { get; set; }

    public RenameCellAction RenameCellHandler { get; set;}

    public void TriggerCellCreated(ICellComponent copyCell)
    {
        OnCellCreatedHandler?.Invoke(copyCell);
    }

    public string RenameCell((int column, int row, int order) index, string name)
    {
        name = RenameCellHandler?.Invoke(index, name) ?? name;
        return name;
    }
}

public delegate string RenameCellAction((int column, int row, int order) index, string oldCellComponentName);