using System.Collections.Generic;

public class Library
{
    public static List<Game> m_Games = new List<Game>();
    public static void InitialiseLibrary()
    {
        Game newGame;

        // Queens gambit accepted: d4, d5, c4, cxc4
        newGame = (new Game("Queens Gambit Accepted"));
        newGame.m_Moves.Add(new Move("d2", "d4"));
        newGame.m_Moves.Add(new Move("d7", "d5"));
        newGame.m_Moves.Add(new Move("c2", "c4"));
        newGame.m_Moves.Add(new Move("P", "d5", "P", "c4"));
        m_Games.Add(newGame);

        // Caro-Cann adcanced variation(bishop exchange): e4, c6, d4, d5, Bf5, Bd3, Bxd3
        newGame = new Game("Caro-Cann: Advanced Variation");
        newGame.m_Moves.Add(new Move("e2", "e4"));
        newGame.m_Moves.Add(new Move("c7", "c6"));
        newGame.m_Moves.Add(new Move("d2", "d4"));
        newGame.m_Moves.Add(new Move("d7", "d5"));
        newGame.m_Moves.Add(new Move("e4", "e5"));
        newGame.m_Moves.Add(new Move("B", "c8", "f5"));
        newGame.m_Moves.Add(new Move("B", "f1", "d3"));
        newGame.m_Moves.Add(new Move("B", "f5", "B", "d3"));
        m_Games.Add(newGame);

        // King's Pawn Game: Wayward Queen Attack (Scholars MAte): e4, e5, Qh4, Nc6, Bc4, Nf6, Qxf7#
        newGame = new Game("King's Pawn Game: Wayward Queen Attack (Scholars Mate)");
        newGame.m_Moves.Add(new Move("e2", "e4"));
        newGame.m_Moves.Add(new Move("e7", "e5"));
        newGame.m_Moves.Add(new Move("Q", "d1", "", "h5"));
        newGame.m_Moves.Add(new Move("N", "b8", "", "c6"));
        newGame.m_Moves.Add(new Move("B", "f1", "", "c4"));
        newGame.m_Moves.Add(new Move("N", "g8", "", "f6"));
        newGame.m_Moves.Add(new Move("Q", "h5", "P", "f7", false, true));
        m_Games.Add(newGame);

        // Scholars Opening: ef4, e6, g4, Qd4#
        newGame = new Game("Bird Opening");
        newGame.m_Moves.Add(new Move("f2", "f4"));
        newGame.m_Moves.Add(new Move("e7", "e6"));
        newGame.m_Moves.Add(new Move("g2", "g4"));
        newGame.m_Moves.Add(new Move("Q", "d8", "", "h4", false, true));
        m_Games.Add(newGame);
    }
}
