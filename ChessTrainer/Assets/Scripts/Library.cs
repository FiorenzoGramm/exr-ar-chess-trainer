using System.Collections.Generic;

public class Library
{
    public static List<Game> games = new List<Game>();
    public static void InitialiseLibrary()
    {
        Game newGame;

        #region Sicilian defense: e4, c5
        newGame = new Game("Sicilian defense");
        newGame.moves.Add(Move.CreateSimplePawnMove("e2", "e4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("c7", "c5"));
        games.Add(newGame);
        #endregion

        #region Queens gambit accepted: d4, d5, c4, cxc4
        newGame = new Game("Queens Gambit Accepted");
        newGame.moves.Add(Move.CreateSimplePawnMove("d2", "d4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("d7", "d5"));
        newGame.moves.Add(Move.CreateSimplePawnMove("c2", "c4"));
        newGame.moves.Add(Move.CreateSimplePawnCapture("d5", 'P', "c4"));
        games.Add(newGame);
        #endregion

        #region Caro-Cann adcanced variation(bishop exchange): e4, c6, d4, d5, e5, Bf5, Bd3, Bxd3
        newGame = new Game("Caro-Cann: Advanced Variation");
        newGame.moves.Add(Move.CreateSimplePawnMove("e2", "e4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("c7", "c6"));
        newGame.moves.Add(Move.CreateSimplePawnMove("d2", "d4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("d7", "d5"));
        newGame.moves.Add(Move.CreateSimplePawnMove("e4", "e5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('B', "c8", "f5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('B', "f1", "d3"));
        newGame.moves.Add(Move.CreateSimplePieceCapture('B', "f5", 'B', "d3"));
        games.Add(newGame);
        #endregion

        #region King's Pawn Game: Wayward Queen Attack (Scholars Mate): e4, e5, Qh5, Nc6, Bc4, Nf6, Qxf7#
        newGame = new Game("King's Pawn Game: Wayward Queen Attack (Scholars Mate)");
        newGame.moves.Add(Move.CreateSimplePawnMove("e2", "e4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("e7", "e5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('Q', "d1", "h5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('N', "b8", "c6"));
        newGame.moves.Add(Move.CreateSimplePieceMove('B', "f1", "c4"));
        newGame.moves.Add(Move.CreateSimplePieceMove('N', "g8", "f6"));
        newGame.moves.Add(Move.CreateFullQualifiedMove('Q', "h5", 'P', "f7", ' ', false, true, "Qxf7#"));
        games.Add(newGame);
        #endregion

        #region Scholars Opening: f4, e6, g4, Qh4#
        newGame = new Game("Bird Opening");
        newGame.moves.Add(Move.CreateSimplePawnMove("f2", "f4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("e7", "e6"));
        newGame.moves.Add(Move.CreateSimplePawnMove("g2", "g4"));
        newGame.moves.Add(Move.CreateFullQualifiedMove('Q', "d8", ' ', "h4", ' ', false, true, "Qh4#"));
        games.Add(newGame);
        #endregion

        #region Testing Bongcloud
        newGame = new Game("Testing Bongcloud");
        newGame.moves.Add(Move.CreateSimplePawnMove("e2", "e4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("e7", "e6"));
        for(int i = 0; i < 100; ++i)
        {
            newGame.moves.Add(Move.CreateSimplePieceMove('K', "e1", "e2"));
            newGame.moves.Add(Move.CreateSimplePieceMove('K', "e8", "e7"));
            newGame.moves.Add(Move.CreateSimplePieceMove('K', "e2", "e1"));
            newGame.moves.Add(Move.CreateSimplePieceMove('K', "e7", "e8"));
        }
        games.Add(newGame);
        #endregion

        #region Fast Queenside castle
        newGame = new Game("Fast Queenside castle");
        newGame.moves.Add(Move.CreateSimplePawnMove("d2", "d4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("d7", "d6"));
        newGame.moves.Add(Move.CreateSimplePawnMove("c2", "c4"));
        newGame.moves.Add(Move.CreateSimplePawnMove("e7", "e5"));
        newGame.moves.Add(Move.CreateSimplePawnMove("d4", "d5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('N', "g8", "f6"));
        newGame.moves.Add(Move.CreateSimplePieceMove('N', "b1", "c3"));
        newGame.moves.Add(Move.CreateSimplePawnMove("g7", "g6"));
        newGame.moves.Add(Move.CreateSimplePieceMove('B', "c1", "g5"));
        newGame.moves.Add(Move.CreateSimplePieceMove('B', "f8", "g7"));
        newGame.moves.Add(Move.CreateSimplePieceMove('Q', "d1", "d2"));
        newGame.moves.Add(Move.CreateFullQualifiedMove('K', "e8", ' ', "g8", ' ', false, false, "O-O"));
        newGame.moves.Add(Move.CreateFullQualifiedMove('K', "e1", ' ', "c1", ' ', false, false, "O-O-O"));
        games.Add(newGame);
        #endregion
    }
}
