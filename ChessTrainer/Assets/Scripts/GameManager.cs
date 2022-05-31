using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Declaration
    public Board        board;
    public List<Board>  boards;
    public Game         currentGame;
    public UIManager    uiManager;

    public Vector3 startPositionBoard;
    public Vector3 startRotationBoard;
    public Vector3 startPositionControllpanel;
    public Vector3 startRotationControllpanel;

    private int currentGameIndex;
    private int currentMoveIndex;
    #endregion

    #region Setup
    void Start()
    {
        Library.InitialiseLibrary();
        try
        {
            ChangeGame(0);
        }
        catch (IndexOutOfRangeException exception)
        {
            Debug.LogError($"Failed to load game: {exception}");
            board.Reset();
        }
        finally
        {
            InitialiseUI();
            PlaceBoardAndUI(); // Prevents the spawning of the board and UI inside the player
        } 
    }

    private void InitialiseUI()
    {
        uiManager.InitialiseUI(Library.GetGamesAsList().Select(game => game.name).ToList(),
                boards.Select(board => board.name).ToList());
    }

    private void PlaceBoardAndUI()
    {
        board.transform.Translate(startPositionBoard, Space.World);
        board.transform.Rotate(startRotationBoard, Space.World);
        uiManager.transform.Translate(startPositionControllpanel, Space.World);
        uiManager.transform.Rotate(startRotationControllpanel, Space.World);
    }
    #endregion

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

    public void ChangeGame(int index)
    {
        board.Reset();
        currentMoveIndex = 0;

        currentGameIndex = index;
        currentGame = Library.GetGameByIndex(currentGameIndex);

        uiManager.GamesHasChanged(currentGame.name);
        Debug.Log($"Changed game to {currentGame.name}");
    }

    public void ChangeTheme(int index)
    {
        int moveIndexBeforeThemeChange = currentMoveIndex;
        Board oldBoard = board;
        board = Instantiate(boards[index]);
        board.transform.position = oldBoard.transform.position;
        board.transform.rotation = oldBoard.transform.rotation;
        board.transform.localScale = oldBoard.transform.localScale;
        board.Reset();
        InitialiseUI();
        currentMoveIndex = 0;
        try
        {
            for (int i = 0; i < moveIndexBeforeThemeChange; ++i)
            {
                DoNextMove();
            }
            Destroy(oldBoard.gameObject);
        }
        catch (Exception exception)
        {
            currentMoveIndex = 0;
            Destroy(board.gameObject);
            board = oldBoard;
            board.Reset();
            for (int i = 0; i < moveIndexBeforeThemeChange; ++i)
            {
                DoNextMove();
            }
            Debug.LogError($"Failed to change theme due failure in move execution: {exception}");
        }
    }

    #region Move execution
    public void DoNextMove()
    {
        Move nextMove;

        try
        {
            nextMove = currentGame.GetMove(currentMoveIndex);
        }
        catch (Exception exception)
        {
            throw exception;
        }

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
        catch (MoveExecutionFailureException exception)
        {
            Debug.LogError($"Failed to execute move {nextMove}: {GetMoveExcecutionFailureToConsoleMessage(exception)}");
        }
    }

    private void ExecuteMove(Move move)
    {
        int[] from = move.GetFromField();
        int[] to = move.GetToField();
        Piece toPiece = null;

        try
        {
            toPiece = board.GetPiece(to);
            TryToClearField(to);
            TryToMovePiece(from, to);
            TryMoveRookForCastle(move);

            if (toPiece != null)
            {
                Destroy(toPiece.gameObject);
            }
        }
        catch(InvalidFieldException exception)
        {
            throw new MoveExecutionFailureException("Failed to execute move due to getting the piece.", move, exception);
        }
        catch (FailedToClearFieldException exception)
        {
            throw new MoveExecutionFailureException("Failed to execute move due to failure in clearing a field.", move, exception);
        }
        catch (FailedToMovePieceException exception)
        {
            board.PlacePieceOnField(toPiece, to);
            throw new MoveExecutionFailureException("Failed to execute move due to failure in moving the piece.", move, exception);
        }
        catch (FailedToMoveRookForCastleException exception)
        {
            board.Move(to, from);
            board.PlacePieceOnField(toPiece, to);
            throw new MoveExecutionFailureException("Failed to execute move due to failure moving the rook for castle.", move, exception);
        }
    }

    private void TryToMovePiece(int[] from, int[] to)
    {
        try
        {
            board.Move(from, to);
        }
        catch (Exception exception)
        {
            throw new FailedToMovePieceException("Failed to move a piece due an exisiting piece on the field.", from, to, exception);
        }
    }

    private void TryToClearField(int[] to)
    {
        try
        {
            board.ClearFieldWithoutDestroying(to);
        }
        catch (Exception exception)
        {
            throw new FailedToClearFieldException("Failed to execute move due to failure in moving the piece.", to, exception);
        }
    }

    private void TryMoveRookForCastle(Move move)
    {
        if (!move.IsCastle())
        {
            return;
        }

        int[] from = new int[2];
        int[] to = new int[2];

        if (IsWhiteMove() && move.IsShortCastle())
        {
            from[0] = 7;
            from[1] = 0;
            to[0]   = 3;
            to[1]   = 0;
        }
        else if (IsWhiteMove() && move.IsLongCastle())
        {
            from[0] = 0;
            from[1] = 0;
            to[0]   = 3;
            to[1]   = 0;
        }
        else if (!IsWhiteMove() && move.IsShortCastle())
        {
            from[0] = 7;
            from[1] = 7;
            to[0]   = 5;
            to[1]   = 7;
        }
        else if (!IsWhiteMove() && move.IsLongCastle())
        {
            from[0] = 7;
            from[1] = 7;
            to[0]   = 4;
            to[1]   = 7;
        }
        try
        {
            TryToMovePiece(from, to);
        }
        catch (Exception exception)
        {
            throw new FailedToMoveRookForCastleException("Failed to move the rook for castle.", from, to, exception);
        }
    }
    #endregion

    #region Move reversal
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
            Debug.Log(currentGame.ToString(currentMoveIndex));
        }
        catch (MoveExecutionFailureException exception)
        {
            Debug.LogError($"Failed to undo move {previousMove}: {GetMoveExcecutionFailureToConsoleMessage(exception)}");
        }
    }

    private void ReverseMove(Move move)
    {
        int[] from = move.GetFromField();
        int[] to = move.GetToField();
        Piece toPiece = CreateCapturedPieceFromMove(move);

        try
        {
            TryToMovePiece(to, from);
            TryToPlacePiece(toPiece, to);
            TryMoveRookForCastleBack(move);
        }
        catch(FailedToMovePieceException exception)
        {
            if (toPiece != null)
            {
                Destroy(toPiece.gameObject);
            }
            throw new FailedToUndoMoveException("Failed undo a move due a failure in moving the piece.", move, exception);
        }
        catch (FailedToPlacePieceException exception)
        {
            if (toPiece != null)
            {
                Destroy(toPiece.gameObject);
            }
            board.Move(from, to);
            throw new FailedToUndoMoveException("Failed undo a move due a failure in placing the captured piece back.", move, exception);
        }
        catch (FailedToMoveRookForCastleException exception)
        {
            if (toPiece != null)
            {
                Destroy(toPiece.gameObject);
            }
            board.Move(from, to);
            throw new FailedToUndoMoveException("Failed undo a move due a failure in the rook for reversing the castle.", move, exception);
        }
    }

    private void TryToPlacePiece(Piece toPiece, int[] to)
    {
        try
        {
            board.PlacePieceOnField(toPiece, to);
        }
        catch(Exception exception)
        {
            throw new FailedToPlacePieceException("Failed to place the piece.", toPiece, to, exception);
        }
    }

    private void TryMoveRookForCastleBack(Move move)
    {
        if (!move.IsCastle())
        {
            return;
        }

        int[] from = new int[2];
        int[] to   = new int[2];

        // The IsWhiteMove must be negated, because it returns the current moves color
        if (!IsWhiteMove() && move.IsShortCastle())
        {
            from[0] = 3;
            from[1] = 0;
            to[0]   = 7;
            to[1]   = 0;
        }
        else if (!IsWhiteMove() && move.IsLongCastle())
        {
            from[0] = 3;
            from[1] = 0;
            to[0]   = 0;
            to[1]   = 0;
        }
        else if (IsWhiteMove() && move.IsShortCastle())
        {
            from[0] = 5;
            from[1] = 7;
            to[0]   = 7;
            to[1]   = 7;
        }
        else if (IsWhiteMove() && move.IsLongCastle())
        {
            from[0] = 5;
            from[1] = 7;
            to[0]   = 7;
            to[1]   = 7;
        }

        try
        {
            TryToMovePiece(from, to);
        }
        catch (Exception exception)
        {
            throw new FailedToMoveRookForCastleException("Failed to move rook to reverse castle.", from, to, exception);
        }
    }
    #endregion

    #region Helper methods
    private string GetMoveExcecutionFailureToConsoleMessage(MoveExecutionFailureException exception)
    {
        StringBuilder message = new StringBuilder();
        message.Append($"Caught error: {exception.Message}:");
        Exception innerException = exception.InnerException;
        if (innerException != null)
        {
            message.Append(innerException);
        }
        return message.ToString();
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

    private bool IsWhiteMove()
    {
        return (currentMoveIndex % 2) == 0;
    }
    #endregion
}
