using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionMenu : MonoBehaviour
{
    #region Declaration
    public UIManager       uiManager;
    public TextMeshPro     titleText;
    public SelectionTile   selectorPrefab;
    public GridLayoutGroup contentObject;
    public Vector3         spawnOnEnableOffset;
    #endregion

    public void Reset()
    {
        DeleteElements();
    }

    public void Initialise(UIManager manager, string title, List<string> namesOfElements, string directoryOfThumbnails)
    {
        uiManager = manager;
        titleText.text = title;
        LoadElements(namesOfElements, directoryOfThumbnails);
    }

    public void OnEnable()
    {
        if(uiManager != null)
        {
            // On instatiation it is null
            transform.position = uiManager.transform.position;
        }
        transform.Translate(spawnOnEnableOffset, Space.Self);
    }

    private void LoadElements(List<string> names, string directoryOfThumbnails)
    {
        DeleteElements();
        for (int i = 0; i < names.Count; ++i)
        {
            string fileName = names[i].Replace(":", "");
            Texture2D thumbnail = Resources.Load($"{directoryOfThumbnails}/{fileName}") as Texture2D;
            SelectionTile currentGameSelectorTile = Instantiate(selectorPrefab, contentObject.transform);
            currentGameSelectorTile.InitialiseTile(this, names[i], thumbnail, i);
        }
    }

    private void DeleteElements()
    {
        foreach (Transform currentTile in contentObject.transform)
        {
            Destroy(currentTile.gameObject);
        }
    }

    public void OnClick(int index)
    {
        uiManager.OnClickOnTile(this, index);
    }
}