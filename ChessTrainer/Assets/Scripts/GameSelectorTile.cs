using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSelectorTile : MonoBehaviour
{
    private TextMeshProUGUI m_TileText;
    private Image           m_Image;
    private RectTransform   m_RectangularTransform;
    private Game            m_Game;
    private GameSelector    m_GameSelector;

    private Color           m_BackgroundColor;
    public  Color           m_BackgroundColorOnTouch;

    public void Awake()
    {
        m_TileText              = GetComponentInChildren<TextMeshProUGUI>();
        m_Image                 = GetComponent<Image>();
        m_RectangularTransform  = GetComponent<RectTransform>();
        

        if (m_TileText is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (TestMeshProUGII)");
        }

        if (m_Image is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (Image)");
        }
        m_BackgroundColor = m_Image.color;

        if (m_RectangularTransform is null)
        {
            throw new MissingComponentException("GameSelectorTile could not be created, because there is a missing component (RectTransform)");
        }
    }

    public void InitialiseTile(GameSelector gameSelector, Game game)
    {
        m_GameSelector  = gameSelector;
        m_Game          = game;
        m_TileText.text = m_Game.m_Name;
    }
    public void ChangeGame()
    {
        m_GameSelector.ChangeCurrentGame(m_Game);
    }

    public void OnTouchBegin()
    {
        m_Image.color = m_BackgroundColorOnTouch;
    }

    public void OnTouchEnd()
    {
        m_Image.color = m_BackgroundColor;
    }
}
