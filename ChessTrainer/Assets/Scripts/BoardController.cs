using Microsoft.MixedReality.Toolkit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    #region Declaration
    public Board boardPrefab;
    public Piece whitePawnPrefab;
    public Piece whiteKingPrefab;
    public Piece whiteQueenPrefab;
    public Piece whiteBishopPrefab;
    public Piece whiteKnightPrefab;
    public Piece whiteRookPrefab;
    public Piece blackPawnPrefab;
    public Piece blackKingPrefab;
    public Piece blackQueenPrefab;
    public Piece blackBishopPrefab;
    public Piece blackKnightPrefab;
    public Piece blackRookPrefab;
    public Field fieldPrefab;

    public List<ChessSet> chessSets;
    private ChessSet currentChessSet;

    public GameManager gameManager;

    private Board board;

    public float FIELD_SIZE; // Length of a square in units
    #endregion

    public void Reset()
    {
        if(currentChessSet == null)
        {
            currentChessSet = chessSets[0];
        }

        if (board != null)
        {
            ClearAllFieldsWithDestroying();
            Destroy(board.gameObject);
        }

        SpawnBoard();
        SpawnPawns();
        SpawnKings();
        SpawnQueens();
        SpawnBishops();
        SpawnKnights();
        SpawnRooks();
        UpdateTheme();
    }

    private void ClearAllFieldsWithDestroying()
    {
        foreach (Field currentField in board.fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null)
            {
                Destroy(currentPiece.gameObject);
                currentField.piece = null;
            }
        }
    }

    #region Initial creation of board and pieces
    private void SpawnBoard()
    {
        board = Instantiate(boardPrefab, transform);
        board.name = "Chessboard";
        board.InitialiseBoard(this);
        SetManipulationTransformOfBoard();
        SetListenersToBoard();
    }

    private void SpawnPawns()
    {
        for (int i = 0; i < 8; ++i)
        {
            SpawnPieceOn(whitePawnPrefab, i, 1);
            SpawnPieceOn(blackPawnPrefab, i, 6);
        }
    }

    private void SpawnKings()
    {
        SpawnPieceOn(whiteKingPrefab, 4, 0);
        SpawnPieceOn(blackKingPrefab, 4, 7);
    }

    private void SpawnQueens()
    {
        SpawnPieceOn(whiteQueenPrefab, 3, 0);
        SpawnPieceOn(blackQueenPrefab, 3, 7);
    }

    private void SpawnBishops()
    {
        SpawnPieceOn(whiteBishopPrefab, 2, 0);
        SpawnPieceOn(whiteBishopPrefab, 5, 0);
        SpawnPieceOn(blackBishopPrefab, 2, 7);
        SpawnPieceOn(blackBishopPrefab, 5, 7);
    }

    private void SpawnKnights()
    {
        SpawnPieceOn(whiteKnightPrefab, 1, 0);
        SpawnPieceOn(whiteKnightPrefab, 6, 0);
        SpawnPieceOn(blackKnightPrefab, 1, 7);
        SpawnPieceOn(blackKnightPrefab, 6, 7);
    }

    public Field GetFieldOfPiece(Piece piece)
    {
        foreach(Field currentField in board.fields)
        {
            Piece currentPiece = currentField.piece;
            if(currentPiece != null && currentPiece.Equals(piece))
            {
                return currentField;
            }
        }
        return null;
    }

    private void SpawnRooks()
    {
        SpawnPieceOn(whiteRookPrefab, 0, 0);
        SpawnPieceOn(whiteRookPrefab, 7, 0);
        SpawnPieceOn(blackRookPrefab, 0, 7);
        SpawnPieceOn(blackRookPrefab, 7, 7);
    }

    private void SpawnPieceOn(Piece prefab, int rank, int file)
    {
        Piece piece = Instantiate(prefab, transform);
        piece.boardController = this;
        if (!piece.isWhite)
        {
            piece.transform.Rotate(Vector3.up, 180f, Space.Self);
        }
        PlacePieceOnPosition(piece, new Vector2Int(rank, file));
    }
    #endregion

    public void CreatePieceOnPosition(bool isWhite, char symbol, Vector2Int position)
    {
        if (!Field.IsValidPosition(position))
        {
            throw new InvalidFieldException($"Failed to create piece due invalid position", position);
        }

        if (isWhite)
        {
            #region Instatiate white prefab
            switch (symbol)
            {
                case 'K':
                    SpawnPieceOn(whiteKingPrefab, position.x, position.y);
                    break;
                case 'Q':
                    SpawnPieceOn(whiteQueenPrefab, position.x, position.y);
                    break;
                case 'R':
                    SpawnPieceOn(whiteRookPrefab, position.x, position.y);
                    break;
                case 'B':
                    SpawnPieceOn(whiteBishopPrefab, position.x, position.y);
                    break;
                case 'N':
                    SpawnPieceOn(whiteKnightPrefab, position.x, position.y);
                    break;
                case 'P':
                    SpawnPieceOn(whitePawnPrefab, position.x, position.y);
                    break;
                default:
                    throw new ArgumentException($"Failed to create piece due invalid symbol {symbol}");
            };
            #endregion
        }
        else
        {
            #region Instatiate black prefab
            switch (symbol)
            {
                case 'K':
                    SpawnPieceOn(blackKingPrefab, position.x, position.y);
                    break;
                case 'Q':
                    SpawnPieceOn(blackQueenPrefab, position.x, position.y);
                    break;
                case 'R':
                    SpawnPieceOn(blackRookPrefab, position.x, position.y);
                    break;
                case 'B':
                    SpawnPieceOn(blackBishopPrefab, position.x, position.y);
                    break;
                case 'N':
                    SpawnPieceOn(blackKnightPrefab, position.x, position.y);
                    break;
                case 'P':
                    SpawnPieceOn(blackPawnPrefab, position.x, position.y);
                    break;
                default:
                    throw new ArgumentException($"Failed to create piece due invalid symbol {symbol}");
            };
            #endregion
        }
        UpdateThemeOfPiece(board.fields[position.x, position.y].piece);
    }

    public void TryNewMovePiece(Piece piece, Field to)
    {
        Field from = GetFieldByPiece(piece);

        if (from == null) 
        {
            throw new ArgumentException($"{piece} is not placed on a field.");
        }

        gameManager.TryNewMove(piece, from, to);
    }

    private Field GetFieldByPiece(Piece piece)
    {
        foreach (Field currentField in board.fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null && currentPiece.Equals(piece))
            {
                return currentField;
            }
        }
        return null;
    }

    public void MoveByPosition(Vector2Int from, Vector2Int to)
    {
        if (!Field.IsValidPosition(from) || !Field.IsValidPosition(from))
        {
            throw new InvalidFieldException($"Cannot not from NULL_FIELD", from);
        }

        Piece fromPiece = board.fields[from.x, from.y].piece;
        Piece toPiece   = board.fields[to.x,   to.y].piece;
        Field fromField = board.fields[from.x, from.y];
        Field toField   = board.fields[to.x,   to.y];

        if (fromPiece == null)
        {
            throw new ThereIsNoPieceException($"There is no piece on {fromField}.", fromField);
        }

        if (toPiece != null)
        {
            throw new FieldAlreadyOccupiedException($"There is a piece on {toField} ({toPiece})", fromPiece, toField);
        }

        PlacePieceOnPosition(fromPiece, to);
        board.fields[from.x, from.y].piece = null;
    }

    public void ClearFieldWithDestroying(Vector2Int to)
    {
        board.DestroyPieceOnPosition(to);
    }

    public void PlacePieceOnPosition(Piece piece, Vector2Int position)
    {
        if (piece == null)
        {
            return;
        }
        if (!Field.IsValidPosition(position))
        {
            throw new InvalidFieldException("Cannot place piece on NULL_FIELD.", position);
        }
        if (!board.IsEmptyFieldByPosition(position))
        {
            throw new FieldAlreadyOccupiedException($"Cannot place {piece} on {position} because the field is occupied by a {board.fields[position.x, position.y].piece}", piece, board.fields[position.x, position.y]);
        }

        // Calculate the position of the piece if the board is at origin with no rotation
        float positionX = transform.localScale.x * FIELD_SIZE * (0.5f + (position.x - 4.0f));
        float positionZ = transform.localScale.z * FIELD_SIZE * (0.5f + (position.y - 4.0f));
        Vector3 newPosition = new Vector3(positionX, 0.0f, positionZ);
        // Apply parent transformation so that the piece could be placed with world coordinates
        newPosition = transform.rotation * newPosition;
        newPosition = transform.position + newPosition;
        //piece.transform.position = newPosition;
        

        piece.GoToPosition(newPosition);
        board.fields[position.x, position.y].piece = piece;
    }

    public List<string> GetListOfThemeNames()
    {
        return chessSets.Select(chessSet => chessSet.name).ToList();
    }

    public void ChangeTheme(int index)
    {
        currentChessSet = chessSets[index];

        UpdateThemeOfBoard();
        UpdateThemeOfAllPieces();
    }

    private void UpdateTheme()
    {
        ChangeTheme(chessSets.IndexOf(currentChessSet));
    }

    private void UpdateThemeOfAllPieces()
    {
        Piece[] pieces = GetComponentsInChildren<Piece>();

        foreach (Piece currentPiece in pieces)
        {
            UpdateThemeOfPiece(currentPiece);
        }
    }

    private void UpdateThemeOfPiece(Piece piece)
    {
        foreach (Transform currentChild in piece.transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                Destroy(currentChild.gameObject);
            }
        }
        if (piece.isWhite)
        {
            #region Instatiate white prefab
            switch (piece.symbol)
            {
                case 'K':
                    Instantiate(currentChessSet.whiteKingModel, piece.transform);
                    break;
                case 'Q':
                    Instantiate(currentChessSet.whiteQueenModel, piece.transform);
                    break;
                case 'R':
                    Instantiate(currentChessSet.whiteRookModel, piece.transform);
                    break;
                case 'B':
                    Instantiate(currentChessSet.whiteBishopModel, piece.transform);
                    break;
                case 'N':
                    Instantiate(currentChessSet.whiteKnightModel, piece.transform);
                    break;
                case 'P':
                    Instantiate(currentChessSet.whitePawnModel, piece.transform);
                    break;
                default:
                    break;
            };
            #endregion
        }
        else
        {
            #region Instatiate black prefab
            switch (piece.symbol)
            {
                case 'K':
                    Instantiate(currentChessSet.blackKingModel, piece.transform);
                    break;
                case 'Q':
                    Instantiate(currentChessSet.blackQueenModel, piece.transform);
                    break;
                case 'R':
                    Instantiate(currentChessSet.blackRookModel, piece.transform);
                    break;
                case 'B':
                    Instantiate(currentChessSet.blackBishopModel, piece.transform);
                    break;
                case 'N':
                    Instantiate(currentChessSet.blackKnightModel, piece.transform);
                    break;
                case 'P':
                    Instantiate(currentChessSet.blackPawnModel, piece.transform);
                    break;
                default:
                    break;
            };
            #endregion
        }
        piece.Initialise();
    }

    private void UpdateThemeOfBoard()
    {
        foreach (Transform currentChild in board.transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                Destroy(currentChild.gameObject);
            }
        }
        GameObject newBoard = Instantiate(currentChessSet.board, board.transform);
        newBoard.name = "Model of Board";
        SetManipulationTransformOfBoard();
        SetListenersToBoard();
        board.UpdateBoxCollider();
    }

    private void SetManipulationTransformOfBoard()
    {
        GetObjectManipulatorFromBoard().HostTransform = transform;
    }

    private void SetListenersToBoard()
    {
        ObjectManipulator objectManipulator = GetObjectManipulatorFromBoard();
        objectManipulator.OnManipulationStarted.AddListener(board.OnGrab);
        objectManipulator.OnManipulationEnded.AddListener(board.OnRelease);
    }

    private ObjectManipulator GetObjectManipulatorFromBoard()
    {
        ObjectManipulator objectManipulator = null;

        foreach (Transform currentChild in board.transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                objectManipulator = currentChild.GetComponent<ObjectManipulator>();
            }
        }

        if (objectManipulator == null)
        {
            throw new MissingComponentException($"Board {gameObject.name} is missing the {typeof(ObjectManipulator)} component.");
        }
        return objectManipulator;
    }

    public void DisableGrabbableOfNonGrabbedPieces()
    {
        board.DisableGrabbableOfNonGrabbedPieces();
    }

    public void EnableGrabbableOfPieces()
    {
        board.EnableGrabbableOfPieces();
    }

    public void DisableGrabbableOfBoard()
    {
        board.DisableGrabbable();
    }

    public void EnableGrabbableOfBoard()
    {
        board.EnableGrabbable();
    }

    public void EnableFieldCollider()
    {
        board.EnableFieldCollider();
    }

    public void DisableFieldCollider()
    {
        board.DisableFieldCollider();
    }

    public void PrintField()
    {
        StringBuilder fieldString = new StringBuilder();

        foreach (Field currentField in board.fields)
        {
            fieldString.Append($"{currentField} : ");

            if (currentField.piece == null)
            {
                fieldString.Append($"-");
            }
            else
            {
                fieldString.Append(currentField.piece.ToString());
            }

            if (currentField.GetRank() == 7)
            {
                if (currentField.GetFile() == 7)
                {
                    fieldString.Append("\n\r");
                }
                else
                {
                    fieldString.Append(", ");
                }
            }
        }
        Debug.Log(fieldString.ToString());
    }
}
