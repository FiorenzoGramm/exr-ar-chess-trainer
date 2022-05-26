using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    public UIManager            m_UIManager;
    public GameSelectorTile     m_SelectorPrefab;
    public VerticalLayoutGroup  m_ContentObject;

    public void LoadGames(ICollection<Game> gamesToSelect)
    {
        DeleteGamesFromSelectionGrid();
        foreach (Game currentGame in gamesToSelect)
        {
            GameSelectorTile currentGameSelectorTile   = Instantiate(m_SelectorPrefab, m_ContentObject.transform);
            currentGameSelectorTile.InitialiseTile(this, currentGame);
        }
    }

    public void DeleteGamesFromSelectionGrid()
    {
        foreach(GameSelectorTile currentTile in m_ContentObject.transform)
        {
            Destroy(currentTile);
        }
    }

    public void ChangeCurrentGame(Game gameToSwitch)
    {
        m_UIManager.ChangeCurrentGame(gameToSwitch);
    }
}
