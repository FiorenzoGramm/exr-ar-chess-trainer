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

    public List<ChessSet> chessSets;
    private ChessSet currentChessSet;

    private Piece[,] Pieces = new Piece[8, 8];

    public float FIELD_SIZE; // Length of a square in units
    #endregion

    public void Reset()
    {
        currentChessSet = chessSets[0];
        // Clear current fields
        for (int rank = 0; rank < 8; ++rank)
        {
            for (int file = 0; file < 8; ++file)
            {
                if (Pieces[rank, file] != null)
                {
                    Destroy(Pieces[rank, file].gameObject);
                    Pieces[rank, file] = null;
                }
            }
        }

        GameObject board = GameObject.FindGameObjectWithTag("Board");
        if(board != null)
        {
            Destroy(board);
        }

        SpawnBoard();
        SpawnPawns();
        SpawnKings();
        SpawnQueens();
        SpawnBishops();
        SpawnKnights();
        SpawnRooks();
    }

    public void PrintField()
    {
        Piece currentPiece;
        StringBuilder fieldString = new StringBuilder();

        for (int file = 7; file >= 0; --file)
        {
            for (int rank = 0; rank < 8; ++rank)
            {
                currentPiece = Pieces[rank, file];

                fieldString.Append($"{FieldToString(rank, file)} : ");

                if (currentPiece == null)
                {
                    fieldString.Append($"-");
                }
                else
                {
                    fieldString.Append(currentPiece.ToString());
                }
                fieldString.Append(", ");
            }
            fieldString.Append("\n\r");
        }
        Debug.Log(fieldString.ToString());
    }

    #region Initial creation of pieces
    private void SpawnBoard()
    {
        Instantiate(boardPrefab, this.transform);
    }
    private void SpawnPawns()
    {
        for (int i = 0; i < 8; ++i)
        {
            Piece whitePawn = Instantiate(whitePawnPrefab, this.transform);
            Piece blackPawn = Instantiate(blackPawnPrefab, this.transform);

            PlacePieceOnField(whitePawn, new Vector2Int(i, 1));
            PlacePieceOnField(blackPawn, new Vector2Int(i, 6));
        }
    }

    private void SpawnKings()
    {
        Piece whiteKing = Instantiate(whiteKingPrefab, this.transform).GetComponent<Piece>();
        Piece blackKing = Instantiate(blackKingPrefab, this.transform).GetComponent<Piece>();

        PlacePieceOnField(whiteKing, new Vector2Int(4, 0));
        PlacePieceOnField(blackKing, new Vector2Int(4, 7));
    }

    private void SpawnQueens()
    {
        Piece whiteQueen = Instantiate(whiteQueenPrefab, this.transform).GetComponent<Piece>();
        Piece blackQueen = Instantiate(blackQueenPrefab, this.transform).GetComponent<Piece>();

        PlacePieceOnField(whiteQueen, new Vector2Int(3, 0));
        PlacePieceOnField(blackQueen, new Vector2Int(3, 7));
    }

    private void SpawnBishops()
    {
        Piece whiteLeftBishop = Instantiate(whiteBishopPrefab, this.transform).GetComponent<Piece>();
        Piece whiteRightBishop = Instantiate(whiteBishopPrefab, this.transform).GetComponent<Piece>();
        Piece blackLeftBishop = Instantiate(blackBishopPrefab, this.transform).GetComponent<Piece>();
        Piece blackRightBishop = Instantiate(blackBishopPrefab, this.transform).GetComponent<Piece>();

        PlacePieceOnField(whiteLeftBishop, new Vector2Int(2, 0));
        PlacePieceOnField(whiteRightBishop, new Vector2Int(5, 0));
        PlacePieceOnField(blackLeftBishop, new Vector2Int(2, 7));
        PlacePieceOnField(blackRightBishop, new Vector2Int(5, 7));
    }

    private void SpawnKnights()
    {
        Piece whiteLeftKnight = Instantiate(whiteKnightPrefab, this.transform).GetComponent<Piece>();
        Piece whiteRightKnight = Instantiate(whiteKnightPrefab, this.transform).GetComponent<Piece>();
        Piece blackLeftKnight = Instantiate(blackKnightPrefab, this.transform).GetComponent<Piece>();
        Piece blackRightKnight = Instantiate(blackKnightPrefab, this.transform).GetComponent<Piece>();

        PlacePieceOnField(whiteLeftKnight, new Vector2Int(1, 0));
        PlacePieceOnField(whiteRightKnight, new Vector2Int(6, 0));
        PlacePieceOnField(blackLeftKnight, new Vector2Int(1, 7));
        PlacePieceOnField(blackRightKnight, new Vector2Int(6, 7));
    }

    private void SpawnRooks()
    {
        Piece whiteLeftRook = Instantiate(whiteRookPrefab, this.transform).GetComponent<Piece>();
        Piece whiteRightRook = Instantiate(whiteRookPrefab, this.transform).GetComponent<Piece>();
        Piece blackLeftRook = Instantiate(blackRookPrefab, this.transform).GetComponent<Piece>();
        Piece blackRightRook = Instantiate(blackRookPrefab, this.transform).GetComponent<Piece>();

        PlacePieceOnField(whiteLeftRook, new Vector2Int(0, 0));
        PlacePieceOnField(whiteRightRook, new Vector2Int(7, 0));
        PlacePieceOnField(blackLeftRook, new Vector2Int(0, 7));
        PlacePieceOnField(blackRightRook, new Vector2Int(7, 7));
    }
    #endregion

    public void Move(Vector2Int from, Vector2Int to)
    {
        if (!IsValidField(from))
        {
            throw new InvalidFieldException($"({from.x}, {from.y}) is not a valid (from) field.", from);
        }
        if (!IsValidField(to))
        {
            throw new InvalidFieldException($"({to[0]}, {to[1]}) is not a valid (to) field.", to);
        }

        Piece fromPiece = Pieces[from.x, from.y];
        Piece toPiece = Pieces[to.x, to.y];

        if (fromPiece == null)
        {
            throw new ThereIsNoPieceException($"There is no piece on {FieldToString(from)}.", from);
        }

        if (toPiece != null)
        {
            throw new FieldAlreadyOccupiedException($"There is a piece on {FieldToString(to)} ({toPiece.ToString()})", fromPiece, to);
        }

        PlacePieceOnField(fromPiece, to);
        Pieces[from[0], from[1]] = null;
    }

    public Piece GetPiece(Vector2Int position)
    {
        if (!IsValidField(position))
        {
            throw new InvalidFieldException($"Failed to get a piece.", position);
        }
        return Pieces[position.x, position.y];
    }

    public void PlacePieceOnField(Piece piece, Vector2Int position)
    {
        if (piece == null)
        {
            return;
        }
        if (!IsValidField(position))
        {
            throw new InvalidFieldException("Failed to place piece due invalid field.", position);
        }
        if (Pieces[position.x, position.y] != null)
        {
            throw new FieldAlreadyOccupiedException($"Cannot place {piece.ToString()} on {FieldToString(position)} because the field is occupied by a {Pieces[position.x, position.y].ToString()}", piece, position);
        }

        // Calculate the position of the piece if the board is at origin with no rotation
        float positionX = transform.localScale.x * FIELD_SIZE * (0.5f + (position.x - 4.0f));
        float positionZ = transform.localScale.z * FIELD_SIZE * (0.5f + (position.y - 4.0f));
        Vector3 newPosition = new Vector3(positionX, 0.0f, positionZ);
        // Apply parent transformation so that the piece could be placed with world coordinates
        newPosition = transform.rotation * newPosition;
        newPosition = transform.position + newPosition;
        piece.transform.position = newPosition;
        Pieces[position.x, position.y] = piece;
    }

    public void ClearFieldWithoutDestroying(Vector2Int position)
    {
        if (!IsValidField(position))
        {
            throw new InvalidFieldException($"Failed to clear field due to invalid field.", position);
        }

        Pieces[position.x, position.y] = null;
    }

    public List<string> GetListOfThemeNames()
    {
        return chessSets.Select(chessSet => chessSet.name).ToList();
    }

    public void ChangeTheme(int index)
    {
        currentChessSet = chessSets[index];

        Board board = GetComponentInChildren<Board>();
        foreach (Transform currentChild in board.transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                Destroy(currentChild.gameObject);
                
            }
        }
        Instantiate(currentChessSet.Board, board.transform);

        Piece[] pieces = GetComponentsInChildren<Piece>();

        foreach(Piece currentPiece in pieces)
        {
            if(currentPiece.CompareTag("Piece"))
            {
                Destroy(currentPiece.transform.GetChild(0).gameObject);
                if (currentPiece.isWhite)
                {
                    #region Instatiate white prefab
                    switch (currentPiece.symbol)
                    {
                        case 'K':
                            Instantiate(currentChessSet.WhiteKingPrefab, currentPiece.transform);
                                break;
                        case 'Q':
                            Instantiate(currentChessSet.WhiteQueenPrefab, currentPiece.transform);
                                break;
                        case 'R':
                            Instantiate(currentChessSet.WhiteRookPrefab, currentPiece.transform);
                                break;
                        case 'B':
                            Instantiate(currentChessSet.WhiteBishopPrefab, currentPiece.transform);
                                break;
                        case 'N':
                            Instantiate(currentChessSet.WhiteKnightPrefab, currentPiece.transform);
                                break;
                        case 'P':
                            Instantiate(currentChessSet.WhitePawnPrefab, currentPiece.transform);
                                break;
                        default:
                            break;
                    };
                    #endregion
                }
                else
                {
                    #region Instatiate black prefab
                    switch (currentPiece.symbol)
                    {
                        case 'K':
                            Instantiate(currentChessSet.BlackKingPrefab, currentPiece.transform);
                            break;
                        case 'Q':
                            Instantiate(currentChessSet.BlackQueenPrefab, currentPiece.transform);
                            break;
                        case 'R':
                            Instantiate(currentChessSet.BlackRookPrefab, currentPiece.transform);
                            break;
                        case 'B':
                            Instantiate(currentChessSet.BlackBishopPrefab, currentPiece.transform);
                            break;
                        case 'N':
                            Instantiate(currentChessSet.BlackKnightPrefab, currentPiece.transform);
                            break;
                        case 'P':
                            Instantiate(currentChessSet.BlackPawnPrefab, currentPiece.transform);
                            break;
                        default:
                            break;
                    };
                    #endregion
                }
            }
        }
    }

    private static bool IsValidField(Vector2Int position)
    {
        return position.x >= 0 && position.x <= 7 && position.y >= 0 && position.y <= 7;
    }

    private static string FieldToString(int rank, int file)
    {
        Vector2Int position = new Vector2Int(rank, file);
        return FieldToString(position);
    }

    private static string FieldToString(Vector2Int position)
    {
        StringBuilder fieldString = new StringBuilder();

        if (!IsValidField(position))
        {
            throw new FailedToConvertFieldException($"Failed to convert field {position} to string.", position);
        }

        fieldString.Append((char)(65 + position.x));
        fieldString.Append((char)(48 + position.y + 1));
        return fieldString.ToString();
    }
}
