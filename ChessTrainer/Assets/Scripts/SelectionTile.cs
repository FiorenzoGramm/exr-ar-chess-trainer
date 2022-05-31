using UnityEngine;
using UnityEngine.UI;

public class SelectionTile : MonoBehaviour
{
    #region Declaration
    public Text     tileText;
    public RawImage image;
    public RectTransform rectangularTransform;

    private int index;
    private SelectionMenu gameSelector;
    #endregion

    public void InitialiseTile(SelectionMenu gameSelector, string gameName, Texture2D thumbnail, int index)
    {
        SetThumbnail(thumbnail);

        this.gameSelector = gameSelector;
        this.index = index;
        tileText.text = gameName;
    }

    private void SetThumbnail(Texture2D thumbnail)
    {
        if (thumbnail != null)
        {
            image.texture = thumbnail;
        }
        // If no image was found, the default image will be used which is defined in the prefab
    }

    #region Method for listener
    public void OnClick()
    {
        gameSelector.OnClick(index);
    }
    #endregion
}