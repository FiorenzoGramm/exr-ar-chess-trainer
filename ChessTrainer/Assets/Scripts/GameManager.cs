using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board     board;
    public Library   library;
    public Game      currentGame;
    public UIManager uiManager;

    public Vector3 startPositionBoard;
    public Vector3 startRotationBoard;
    public Vector3 startPositionControllpanel;
    public Vector3 startRotationControllpanel;

    private int currentGameIndex;
    private int currentMoveIndex;

    void Start()
    {
        Library.InitialiseLibrary();
        try
        {
            ChangeGame(0);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception);
            board.Reset();
        }
        finally
        {
            uiManager.InitialiseUI(Library.games);
            PlaceBoardAndUI(); // Prevents the spawning of the board and UI inside the player
        } 
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            UndoMove();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            DoNextMove();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            ChangeGame(currentGameIndex + 1);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            ChangeGame(currentGameIndex - 1);
        }
        if (Input.GetKeyUp(KeyCode.P))
        {
            board.PrintField();
        }
    }

    private void PlaceBoardAndUI()
    {
        board.transform.Translate(startPositionBoard, Space.World);
        board.transform.Rotate(startRotationBoard, Space.World);
        uiManager.transform.Translate(startPositionControllpanel, Space.World);
        uiManager.transform.Rotate(startRotationControllpanel, Space.World);
    }

    public void ChangeGame(int index)
    {
        if(!IsValidGameIndex(index))
        {
            throw new InvalidGameException($"Could not load game with index {index}");
        }

        board.Reset();
        currentMoveIndex = 0;

        currentGameIndex = index;
        currentGame = Library.games[currentGameIndex];
        
        uiManager.GamesHasChanged(currentGame);
        Debug.Log($"Changed game to {currentGame.name}");
    }

    public void DoNextMove()
    {
        Move nextMove = currentGame.GetMove(currentMoveIndex);

        if(nextMove == null)
        {
            Debug.Log("No moves left.");
            return;
        }

        try
        {
            ExecuteMove(nextMove);
            ++currentMoveIndex;
            uiManager.MoveDone(currentMoveIndex - 1, nextMove.ToString());
            Debug.Log(currentGame.ToString(currentMoveIndex));
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to execute move: {exception}");
            Debug.LogError(exception.StackTrace);
        }
    }

    private void ExecuteMove(Move move)
    {
        int[] from = move.GetFromField();
        int[] to = move.GetToField();
        Piece toPiece = board.GetPiece(to[0], to[1]);

        try
        {
            board.ClearFieldWithoutDestroying(to[0], to[1]);
            try
            {
                board.Move(from[0], from[1], to[0], to[1]);
                try
                {
                    TryMoveRookForCastle(move);
                    if (toPiece != null)
                    {
                        Destroy(toPiece.gameObject);
                    }
                }
                catch (Exception exception)
                {
                    board.Move(to[0], to[1], from[0], from[1]);
                    throw exception;
                }
            }
            catch (Exception exception)
            {
                board.PlacePieceOnField(toPiece, to[0], to[1]);
                throw new Exception($"Cannot move piece from ({from[0]}, {from[1]}) on field ({to[0]}, {to[1]}): {exception.Message}");
            }
        }
        catch (Exception exception)
        {
            throw new Exception($"Cannot not execute move:{exception.Message}");
        }
    }

    private void TryMoveRookForCastle(Move move)
    {
        if (!move.IsCastle())
        {
            return;
        }

        try
        {
            if (IsWhiteMove() && move.IsShortCastle())
            {
                board.Move(7, 0, 3, 0);
            }
            else if (IsWhiteMove() && move.IsLongCastle())
            {
                board.Move(0, 0, 3, 0);
            }
            else if (!IsWhiteMove() && move.IsShortCastle())
            {
                board.Move(7, 7, 5, 7);
            }
            else if (!IsWhiteMove() && move.IsLongCastle())
            {
                board.Move(7, 7, 5, 7);
            }
        }
        catch (Exception exception)
        {
            throw new Exception($"Cannot move rooks to castle: {exception.Message}");
        }
    }

    public void UndoMove()
    {
        if (currentMoveIndex <= 0)
        {
            Debug.Log("You are already at the start of the game.");
            return;
        }
        
        Move previousMove   = currentGame.GetMove(currentMoveIndex - 1);

        try
        {
            ReverseMove(previousMove);
            --currentMoveIndex;
            uiManager.MoveUnDone();
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to undo move: {exception}");
            Debug.LogError(exception.StackTrace);
        }

    }

    private void ReverseMove(Move move)
    {
        int[] from = move.GetFromField();
        int[] to = move.GetToField();
        Piece toPiece = CreateCapturedPieceFromMove(move);

        try
        {
            board.Move(to[0], to[1], from[0], from[1]);
            try
            {
                TryMoveRookForCastleBack(move);
                board.PlacePieceOnField(toPiece, to[0], to[1]);
            }
            catch (Exception exception)
            {
                board.Move(from[0], from[1], to[0], to[1]);
                if (toPiece != null)
                {
                    Destroy(toPiece.gameObject);
                }
                throw exception;
            }
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to move pieces back: {exception}");
        }
    }

    private void TryMoveRookForCastleBack(Move move)
    {
        if (!move.IsCastle())
        {
            return;
        }

        try
        {
            // The IsWhiteMove must be negated, because it returns the current moves color!
            if (!IsWhiteMove() && move.IsShortCastle())
            {
                board.Move(3, 0, 7, 0);
            }
            else if (!IsWhiteMove() && move.IsLongCastle())
            {
                board.Move(3, 0, 0, 0);
            }
            else if (IsWhiteMove() && move.IsShortCastle())
            {
                board.Move(5, 7, 7, 7);
            }
            else if (IsWhiteMove() && move.IsLongCastle())
            {
                board.Move(5, 7, 7, 7);
            }
        }
        catch (Exception exception)
        {
            throw new Exception($"Failed to move the rook back: {exception.Message}");
        }

    }

    private Piece CreateCapturedPieceFromMove(Move move)
    {
        Piece capturedPiece;
        Transform boardTransform = board.GetComponent<Transform>();
        
        if (IsWhiteMove())
        {
            #region Instatiate white prefab
            capturedPiece = move.GetToPiece() switch 
            {
                'K' => Instantiate(board.WhiteKingPrefab,   boardTransform),
                'Q' => Instantiate(board.WhiteQueenPrefab,  boardTransform),
                'R' => Instantiate(board.WhiteRookPrefab,   boardTransform),
                'B' => Instantiate(board.WhiteBishopPrefab, boardTransform),
                'N' => Instantiate(board.WhiteKnightPrefab, boardTransform),
                'P' => Instantiate(board.WhitePawnPrefab,   boardTransform),
                _   => null
            };
            #endregion
        }
        else
        {
            #region Instatiate black prefab
            capturedPiece = move.GetToPiece() switch
            {
                'K' => Instantiate(board.BlackKingPrefab,   boardTransform),
                'Q' => Instantiate(board.BlackQueenPrefab,  boardTransform),
                'R' => Instantiate(board.BlackRookPrefab,   boardTransform),
                'B' => Instantiate(board.BlackBishopPrefab, boardTransform),
                'N' => Instantiate(board.BlackKnightPrefab, boardTransform),
                'P' => Instantiate(board.BlackPawnPrefab,   boardTransform),
                _   => null
            };
            #endregion
        }
        return capturedPiece;
    }

    private bool IsValidGameIndex(int index)
    {
        return index >= 0 && index < Library.games.Count;
    }

    private bool IsWhiteMove()
    {
        return (currentMoveIndex % 2) == 0;
    }
}
