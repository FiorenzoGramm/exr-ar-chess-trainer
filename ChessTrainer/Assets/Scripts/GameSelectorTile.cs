using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSelectorTile : MonoBehaviour
{
    public Text tileText;
    public Image image;
    public RectTransform rectangularTransform;
    private Game game;
    private int gameIndex;
    private GameSelector gameSelector;

    public void InitialiseTile(GameSelector gameSelector, Game game, int index)
    {
        tileText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        rectangularTransform = GetComponent<RectTransform>();

        if (tileText is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (Test)");
        }

        if (image is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (Image)");
        }

        if (rectangularTransform is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (RectTransform)");
        }

        this.gameSelector = gameSelector;
        this.game = game;
        gameIndex = index;
        tileText.text = this.game.name;
    }
    public void ChangeGame()
    {
        gameSelector.ChangeGame(gameIndex);
    }
}