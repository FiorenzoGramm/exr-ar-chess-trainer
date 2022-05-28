using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameManager gameMangager;
    public GameSelector gameSelectionMenu;
    public TextMeshPro gameInfoName;
    public GameHistoryController gameHistoryController;

    public void InitialiseUI(List<Game> games)
    {
        gameSelectionMenu.LoadGames(games);
        gameInfoName.text = gameMangager.currentGame.name;
    }

    public void ChangeGame(int index)
    {
        gameMangager.ChangeGame(index);
    }

    public void GamesHasChanged(Game game)
    {
        gameInfoName.text = game.name;
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
}
