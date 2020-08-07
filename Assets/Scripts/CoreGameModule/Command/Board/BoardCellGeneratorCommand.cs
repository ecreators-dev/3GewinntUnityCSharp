using System;
using UnityEngine;

public class BoardCellGeneratorCommand : GameCommand<BoardParameters>
{
    public override void Execute(BoardParameters parameter)
    {
        RemoveAllCells(parameter.TransformParent);

        CreateCells(parameter);
    }

    private void CreateCells(BoardParameters parameter)
    {
        var prefab = parameter.ZelleVorlage;
        var width = prefab.transform.localScale.x;
        var height = prefab.transform.localScale.y;
        var totalWidth = width * parameter.Spalten + parameter.Abstand * (parameter.Spalten - 1);
        var totalHeight = height * parameter.Zeilen + parameter.Abstand * (parameter.Zeilen - 1);

        int order = 0;
        for (int row = parameter.Zeilen - 1; row >= 0; row--)
        {
            for (int column = 0; column < parameter.Spalten; column++)
            {
                (int column, int row) index = (column, row);
                float x = column * (width + parameter.Abstand) - totalWidth / 2;
                float y = row * (height + parameter.Abstand) - totalHeight / 2;
                var position = new Vector3(x, y);
                ICellComponent copyCell = UnityEngine.Object.Instantiate(prefab.GetSelfComponent(), position, Quaternion.identity, parameter.TransformParent).GetComponent<ICellComponent>();
                copyCell.Index = index;
                copyCell.GetSelfComponent().name = parameter.RenameCell((column, row, order), copyCell.GetSelfComponent().name);
                parameter.TriggerCellCreated(copyCell);
                order++;
            }
        }
    }

    public void RemoveAllCells(Transform transformBoard)
    {
        for(int i = transformBoard.childCount - 1; i >= 0; i--)
        {
            var child = transformBoard.GetChild(i);
            if (child.TryGetComponent(out ICellComponent zelle))
            {
                Delete(zelle);
            }
        }
    }
}