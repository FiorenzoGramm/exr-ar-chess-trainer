using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    #region Declaration
    public  GameManager   gameMangager;
    public  SelectionMenu selectionMenuPrefab;
    private SelectionMenu gameSelection;
    private SelectionMenu designSelection;
    private SelectionMenu themeSelection;

    public TextMeshPro           gameInfoName;
    public GameHistoryController gameHistoryController;

    public List<Material> designs;
    public Material       currentDesign;

    public string directoryOfGamesThumbnail;
    public string directoryOfDesignThumbnail;
    public string directoryOfThemeThumbnail;
    #endregion

    private void Reset()
    {
        if(gameSelection != null)
        {
            Destroy(gameSelection.gameObject);
            gameSelection = null;
        }
        if (designSelection != null)
        {
            Destroy(designSelection.gameObject);
            designSelection = null;
        }
        if (themeSelection != null)
        {
            Destroy(themeSelection.gameObject);
            themeSelection = null;
        }
        gameHistoryController.Reset();
    }

    public void InitialiseUI(List<string> games, List<string> themes)
    {
        Reset();
        ResetSelectionMenu(ref gameSelection,   "Openings", games, directoryOfGamesThumbnail);
        ResetSelectionMenu(ref designSelection, "Designs",  designs.Select(material => material.name).ToList(), directoryOfDesignThumbnail);
        ResetSelectionMenu(ref themeSelection,  "Themes",   themes, directoryOfThemeThumbnail);
        gameInfoName.text = gameMangager.currentGame.name;
    }

    private void ResetSelectionMenu(ref SelectionMenu menu, string title, List<string> namesOfElements, string directory)
    {
        foreach(string s in namesOfElements)
        {
            Debug.Log(s);
        }
        if (menu != null)
        {
            Destroy(menu.gameObject);
        }

        menu = Instantiate(selectionMenuPrefab, transform);
        menu.name = title;
        menu.Initialise(this, title, namesOfElements, directory);
        menu.gameObject.SetActive(false);
    }

    public void OnClickOnTile(SelectionMenu menu, int index)
    {
        if (menu.Equals(gameSelection))
        {
            gameMangager.ChangeGame(index);
        }
        if (menu.Equals(designSelection))
        {
            ChangeDesign(index);
        }
        if (menu.Equals(themeSelection))
        {
            gameMangager.ChangeTheme(index);
        }
    }

    private void ChangeDesign(int index)
    {
        Material newDesign = designs[index];
        UpdateDesign(this.transform, newDesign);
        UpdateDesign(gameSelection.transform, newDesign);
        UpdateDesign(designSelection.transform, newDesign);
        UpdateDesign(themeSelection.transform, newDesign);
        currentDesign = newDesign;
    }

    private void UpdateDesign(Transform objTransform, Material newDesign)
    {
        Renderer[] allMeshes = objTransform.GetComponentsInChildren<Renderer>();

        foreach (Renderer currentMesh in allMeshes)
        {
            if (currentMesh.sharedMaterials[0].Equals(currentDesign))
            {
                currentMesh.material = newDesign;
            }
        }
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

    #region Methods for the listeners
    public void OpenGameSelection()
    {
        gameSelection.gameObject.SetActive(!gameSelection.gameObject.activeSelf);
    }
    public void OpenDesignSelection()
    {
        designSelection.gameObject.SetActive(!designSelection.gameObject.activeSelf);
    }
    public void OpenThemeSelection()
    {
        themeSelection.gameObject.SetActive(!themeSelection.gameObject.activeSelf);
    }
    #endregion
}
