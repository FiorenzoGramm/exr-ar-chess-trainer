using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    public UIManager uiManager;
    public GameSelectorTile selectorPrefab;
    public GridLayoutGroup contentObject;
    public Vector3 spawnOnEnableOffset;

    public void Reset()
    {
        DeleteGamesFromSelectionGrid();
    }

    public void LoadGames(List<string> nameOfGames)
    {
        DeleteGamesFromSelectionGrid();
        for (int i = 0; i < nameOfGames.Count; ++i)
        {
            GameSelectorTile currentGameSelectorTile = Instantiate(selectorPrefab, contentObject.transform);
            currentGameSelectorTile.InitialiseTile(this, nameOfGames[i], i);
        }
    }

    public void OnEnable()
    {
        transform.position = uiManager.transform.position + spawnOnEnableOffset;
        transform.rotation = uiManager.transform.rotation;
    }

    public void DeleteGamesFromSelectionGrid()
    {
        foreach (Transform currentTile in contentObject.transform)
        {
            Destroy(currentTile.gameObject);
        }
    }

    public void ChangeGame(int index)
    {
        uiManager.ChangeGame(index);
    }
}