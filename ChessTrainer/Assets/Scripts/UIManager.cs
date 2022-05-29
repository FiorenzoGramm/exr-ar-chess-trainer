using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager gameMangager;
    public GameSelector gameSelectionMenu;
    public TextMeshPro gameInfoName;
    public GameHistoryController gameHistoryController;
    public List<Material> Designs;
    public Material MaterialToReplace;
    public Material CurrentTheme;
    public MeshRenderer Backplate;
    private int CurrentDesignIndex;

    public void Reset()
    {
        gameSelectionMenu.Reset();
        gameHistoryController.Reset();
    }
    public void InitialiseUI(List<string> games)
    {
        Reset();
        gameSelectionMenu.LoadGames(games);
        CurrentDesignIndex = Designs.IndexOf(CurrentTheme);
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

    public void ChangeDesign()
    {
        if (CurrentDesignIndex == Designs.Count - 1)
        {
            CurrentDesignIndex = 0;
        }
        else
        {
            ++CurrentDesignIndex;
        }

        Renderer[] allMeshes = GetComponentsInChildren<Renderer>();

        foreach(Renderer currentMesh in allMeshes)
        {
            if (CurrentTheme.Equals(currentMesh.sharedMaterials[0]))
            {
                currentMesh.material = Designs[CurrentDesignIndex];
            }
        }

        allMeshes = gameSelectionMenu.GetComponentsInChildren<Renderer>();

        foreach (Renderer currentMesh in allMeshes)
        {
            if (CurrentTheme.Equals(currentMesh.sharedMaterials[0]))
            {
                currentMesh.material = Designs[CurrentDesignIndex];
            }
        }

        CurrentTheme = Designs[CurrentDesignIndex];
    }

    public void ChangeThemeOfBoard()
    {
        gameMangager.ChangeThemeOfBoard();
    }
}
