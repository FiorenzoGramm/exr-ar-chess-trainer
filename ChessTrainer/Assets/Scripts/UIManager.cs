using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameManager  m_GameManager;
    public GameSelector m_GameSelector;

    public void InitialiseUI(ICollection<Game> games)
    {
        if(!FindAndSetGamemanager())
        {
            throw new UnityException("There is no GameManager found.");
        }
        m_GameSelector.LoadGames(games);    
    }

    private bool FindAndSetGamemanager()
    {
        if(m_GameManager is not null)
        {
            return true;
        }
        m_GameManager = FindObjectOfType<GameManager>();
        return m_GameManager is not null;
    }

    public void ChangeCurrentGame(Game gameToSwitch)
    {
        m_GameManager.ChangeGame(gameToSwitch);
    }
}
