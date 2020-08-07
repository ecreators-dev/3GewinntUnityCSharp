using Assets.Scripts.GameplayModule.Scriptable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardComponent : MonoBehaviour
{
    public int spalten = 10;
    public int zeilen = 10;
    public ZelleComponent zelleVorlage;
    public ContentCreationConfig[] poolData;
    public bool contentErstBeiStart = true;
    public LevelConfig levelChallenge;
    private bool showingChallengeChange;
    private int showingChallengeMatches;
    public static readonly BoardCellGeneratorCommand boardCreationCommand = new BoardCellGeneratorCommand();
    public static readonly CellInitCommand cellInitCommand = new CellInitCommand();

    [ContextMenu("Zellen entfernen")]
    public void RemoveCells()
    {
        boardCreationCommand.RemoveAllCells(transform);
    }

    [ContextMenu("Zellen Konstruieren")]
    public void BuildCells()
    {
        boardCreationCommand.Execute(new BoardParameters
        {
            Spalten = spalten,
            Zeilen = zeilen,
            Abstand = 0.1f,
            TransformParent = transform,
            ZelleVorlage = zelleVorlage,
            OnCellCreatedHandler = this.OnCellCreated,
            RenameCellHandler = RenameCell
        });

        Debug.Log($"Alle Zellen ({spalten * zeilen}) erstellt");
    }

    private string RenameCell((int column, int row, int order) index, string oldCellComponentName)
    {
        return $"Zelle[#{index.order}x:y{index.column}:{index.row}]";
    }

    private void OnCellCreated(ICellComponent newCell)
    {
        Debug.Log($"Zelle erstellt in Coordinate {newCell.Index}");

        newCell.ContentReachedCellEvent += EvaluateMatchOfCellContentInCell;

        if (contentErstBeiStart)
        {
            return;
        }

        CreateContentInCell(newCell);
    }

    private void EvaluateMatchOfCellContentInCell(IContentComponent contentInCellNotMoving)
    {
        (EContentType type, int amount)[] matches = GetMatches(contentInCellNotMoving);

        // calculation - if animation or game stops, calculation is updated
        levelChallenge.AddSolved(matches);

        // show calculation
        ShowChallengeChanges(matches);

        // validation:
        ValidateEndOfGame();
    }

    private void ShowChallengeChanges((EContentType type, int amount)[] matches)
    {
        var challengeMatches = matches
            .Where(m => levelChallenge.Challenges.Any(c => c.type == m.type))
            .ToList();
        
        ShowChallengeChange(challengeMatches);
    }

    private void ValidateEndOfGame()
    {
        if (levelChallenge.IsVictory)
        {
            HandleVictory();
        }
        else if (levelChallenge.IsEnd)
        {
            HandleGameOver();
        }
    }

    private (EContentType ContentType, int amount)[] GetMatches(IContentComponent contentToValidate)
    {
        var resultMatches = new List<(EContentType ContentType, int amount)>();

        // TODO - find matches!

        return resultMatches.ToArray();
    }

    private void ShowChallengeChange(List<(EContentType type, int amount)> lists)
    {
        showingChallengeChange = true;
        showingChallengeMatches = lists.Count;
        lists.ForEach(match => StartCoroutine(ShowChallengeMatchAnimation(match)));
    }

    private IEnumerator ShowChallengeMatchAnimation((EContentType type, int amount) match)
    {
        // TODO - Animation implementation
        yield return null;

        showingChallengeMatches--;
        if (showingChallengeMatches == 0)
        {
            showingChallengeChange = false;
        }
    }

    private void HandleGameOver() => StartCoroutine(WaitForShow(ShowGameOver));

    private IEnumerator WaitForShow(Action then)
    {
        yield return new WaitUntil(() => showingChallengeChange == false);

        then.Invoke();
    }

    private void HandleVictory() => StartCoroutine(WaitForShow(ShowVictory));

    private void ShowVictory()
    {
        throw new NotImplementedException();
    }

    private void ShowGameOver()
    {
        throw new NotImplementedException();
    }

    private void CreateContentInCell(ICellComponent newCell)
    {
        StartCoroutine(CreateContentInCell(new CellInitParameters
        {
            PoolData = poolData.Cast<IContentCreationConfig>().ToArray(),
            ZelleCopy = newCell,
            FindCellWithContentTypeHandler = FindCellsWithContentType,
            OnCellContentCreatedHandler = OnCellContentCreated
        }));
    }

    private void OnCellContentCreated(IContentComponent content)
    {
        Debug.Log($"Content erstellt in Zelle {content.Cell.Index}");
    }

    private (int column, int row)[] FindCellsWithContentType(EContentType contentType)
    {
        return (from content in FindObjectsOfType<ZellInhaltComponent>()
                where content.contentType == contentType
                select content.Cell.Index).ToArray();
    }

    private IEnumerator CreateContentInCell(CellInitParameters cellInitParameters)
    {
        yield return new WaitUntil(() =>
        {
            cellInitCommand.Execute(cellInitParameters);
            return true;
        });
    }

    // Use this for initialization
    void Start()
    {
        if (contentErstBeiStart)
        {
            StartCoroutine(CreateContentInCells());
        }
    }

    private IEnumerator CreateContentInCells()
    {
        foreach (var child in transform)
        {
            if (child is MonoBehaviour mb && mb.TryGetComponent(out ZelleComponent zelle))
            {
                CreateContentInCell(zelle);
            }
            yield return null;
        }
    }
}