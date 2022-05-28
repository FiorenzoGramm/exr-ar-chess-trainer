using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class GameSelectorTile : MonoBehaviour
{
    public Text tileText;
    public RawImage image;
    public RectTransform rectangularTransform;
    public string directoryOfOpenings;

    private int gameIndex;
    private GameSelector gameSelector;

    public void InitialiseTile(GameSelector gameSelector, string gameName, int index)
    {
        if (tileText is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (Test)");
        }

        if (image is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (Image)");
        }

        string fileName = gameName.Replace(":", "");
        Texture2D newImage = Resources.Load($"{directoryOfOpenings}/{fileName}") as Texture2D;
        if(newImage != null)
        {
            image.texture = newImage;
        }

        if (rectangularTransform is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (RectTransform)");
        }

        this.gameSelector = gameSelector;
        gameIndex = index;
        tileText.text = gameName;
    }
    public void ChangeGame()
    {
        gameSelector.ChangeGame(gameIndex);
    }
}