using UnityEngine;
using UnityEngine.UI;

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
        LoadThumbnail(gameName);

        this.gameSelector = gameSelector;
        gameIndex = index;
        tileText.text = gameName;
    }

    private void LoadThumbnail(string gameName)
    {
        string fileName = gameName.Replace(":", "");
        Texture2D newImage = Resources.Load($"{directoryOfOpenings}/{fileName}") as Texture2D;
        if (newImage != null)
        {
            image.texture = newImage;
        }
        // If no image was found, the default image will be used which is defined in the prefab
    }

    public void ChangeGame()
    {
        gameSelector.ChangeGame(gameIndex);
    }
}