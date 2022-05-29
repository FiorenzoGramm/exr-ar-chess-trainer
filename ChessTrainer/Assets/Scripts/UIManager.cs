using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager gameMangager;
    public GameSelector gameSelectionMenu;
    public TextMeshPro gameInfoName;
    public GameHistoryController gameHistoryController;
    public List<Material> Themes;
    public Material MaterialToReplace;
    public Material CurrentTheme;
    public MeshRenderer Backplate;
    private int CurrentThemeIndex;
    public void InitialiseUI(List<string> games)
    {
        gameSelectionMenu.LoadGames(games);
        CurrentThemeIndex = Themes.IndexOf(CurrentTheme);
        gameInfoName.text = gameMangager.currentGame.name;
    }

    public void ChangeGame(int index)
    {
        gameMangager.ChangeGame(index);
    }

    public void GamesHasChanged(string nameOfTheGame)
    {
        gameInfoName.text = nameOfTheGame;
        gameHistoryController.Reset();
    }

    public void MoveDone(int index, string move)
    {
        gameHistoryController.AddMove(index, move);
    }

    public void MoveUnDone()
    {
        gameHistoryController.RemoveMove();
    }

    public void ChangeTheme()
    {
        Debug.LogWarning("Triggered");

        if (CurrentThemeIndex == Themes.Count - 1)
        {
            CurrentThemeIndex = 0;
        }
        else
        {
            ++CurrentThemeIndex;
        }

        Renderer[] allMeshes = GetComponentsInChildren<Renderer>();

        foreach(Renderer currentMesh in allMeshes)
        {
            if (CurrentTheme.Equals(currentMesh.sharedMaterials[0]))
            {
                Debug.LogWarning(currentMesh.name);
                currentMesh.material = Themes[CurrentThemeIndex];
            }
        }

        allMeshes = gameSelectionMenu.GetComponentsInChildren<Renderer>();

        foreach (Renderer currentMesh in allMeshes)
        {
            if (CurrentTheme.Equals(currentMesh.sharedMaterials[0]))
            {
                Debug.LogWarning(currentMesh.name);
                currentMesh.material = Themes[CurrentThemeIndex];
            }
        }

        CurrentTheme = Themes[CurrentThemeIndex];
    }
}
