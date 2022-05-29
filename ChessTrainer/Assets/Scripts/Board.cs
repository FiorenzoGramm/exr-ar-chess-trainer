using System.Text;
using UnityEngine;

public class Board : MonoBehaviour
{
    // White pieces (prefabs)
    public Piece WhitePawnPrefab;
    public Piece WhiteKingPrefab;
    public Piece WhiteQueenPrefab;
    public Piece WhiteBishopPrefab;
    public Piece WhiteKnightPrefab;
    public Piece WhiteRookPrefab;
    // Black pieces (prefabs)
    public Piece BlackPawnPrefab;
    public Piece BlackKingPrefab;
    public Piece BlackQueenPrefab;
    public Piece BlackBishopPrefab;
    public Piece BlackKnightPrefab;
    public Piece BlackRookPrefab;

    private Piece[,] Pieces = new Piece[8,8];

    public const float FIELD_SIZE = 0.06f / 100f; // Length of a square in units

    public void Reset()
    {
        // Clear current fields
        for(int rank = 0; rank < 8; ++rank)
        {
            for(int file = 0; file < 8; ++file)
            {
                if(Pieces[rank, file] != null)
                {
                    Destroy(Pieces[rank, file].gameObject);
                    Pieces[rank, file] = null;
                }
            }
        }

        // Spawn Pieces
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
    private void SpawnPawns()
    {
        for (int i = 0; i < 8; ++i)
        {
            Piece whitePawn = Instantiate(WhitePawnPrefab, this.transform);
            Piece blackPawn = Instantiate(BlackPawnPrefab, this.transform);

            PlacePieceOnField(whitePawn, i, 1);
            PlacePieceOnField(blackPawn, i, 6);
        }
    }

    private void SpawnKings()
    {
        Piece whiteKing = Instantiate(WhiteKingPrefab, this.transform);
        Piece blackKing = Instantiate(BlackKingPrefab, this.transform);

        PlacePieceOnField(whiteKing, 4 ,0);
        PlacePieceOnField(blackKing, 4 ,7);
    }

    private void SpawnQueens()
    {
        Piece whiteQueen = Instantiate(WhiteQueenPrefab, this.transform);
        Piece blackQueen = Instantiate(BlackQueenPrefab, this.transform);

        PlacePieceOnField(whiteQueen, 3, 0);
        PlacePieceOnField(blackQueen, 3, 7);
    }

    private void SpawnBishops()
    {
        Piece whiteLeftBishop   = Instantiate(WhiteBishopPrefab, this.transform);
        Piece whiteRightBishop  = Instantiate(WhiteBishopPrefab, this.transform);
        Piece blackLeftBishop   = Instantiate(BlackBishopPrefab, this.transform);
        Piece blackRightBishop  = Instantiate(BlackBishopPrefab, this.transform);

        PlacePieceOnField(whiteLeftBishop,  2, 0);
        PlacePieceOnField(whiteRightBishop, 5, 0);
        PlacePieceOnField(blackLeftBishop,  2, 7);
        PlacePieceOnField(blackRightBishop, 5, 7);
    }

    private void SpawnKnights()
    {
        Piece whiteLeftKnight   = Instantiate(WhiteKnightPrefab, this.transform);
        Piece whiteRightKnight  = Instantiate(WhiteKnightPrefab, this.transform);
        Piece blackLeftKnight   = Instantiate(BlackKnightPrefab, this.transform);
        Piece blackRightKnight  = Instantiate(BlackKnightPrefab, this.transform);

        PlacePieceOnField(whiteLeftKnight,  1, 0);
        PlacePieceOnField(whiteRightKnight, 6, 0);
        PlacePieceOnField(blackLeftKnight,  1, 7);
        PlacePieceOnField(blackRightKnight, 6, 7);
    }

    private void SpawnRooks()
    {
        Piece whiteLeftRook     = Instantiate(WhiteRookPrefab, this.transform);
        Piece whiteRightRook    = Instantiate(WhiteRookPrefab, this.transform);
        Piece blackLeftRook     = Instantiate(BlackRookPrefab, this.transform);
        Piece blackRightRook    = Instantiate(BlackRookPrefab, this.transform);

        PlacePieceOnField(whiteLeftRook,    0, 0);
        PlacePieceOnField(whiteRightRook,   7, 0);
        PlacePieceOnField(blackLeftRook,    0, 7);
        PlacePieceOnField(blackRightRook,   7, 7);
    }
    #endregion

    public void Move(int[] from, int[] to)
    {
        if(!IsValidField(from))
        {
            throw new InvalidFieldException($"({from[0]}, {from[1]}) is not a valid (from) field.", from);
        }
        if (!IsValidField(to))
        {
            throw new InvalidFieldException($"({to[0]}, {to[1]}) is not a valid (to) field.", to);
        }

        Piece fromPiece = Pieces[from[0], from[1]];
        Piece toPiece   = Pieces[to[0], to[1]];

        if(fromPiece == null)
        {
            throw new ThereIsNoPieceException($"There is no piece on {FieldToString(from)}.", from);
        }

        if(toPiece != null)
        {
            throw new FieldAlreadyOccupiedException($"There is a piece on {FieldToString(to)} ({toPiece.ToString()})", fromPiece, to);
        }

        PlacePieceOnField(fromPiece, to);
        Pieces[from[0], from[1]] = null;
    }

    public Piece GetPiece(int[] field)
    {
        if (!IsValidField(field))
        {
            throw new InvalidFieldException($"Failed to get a piece.", field);
        }
        return Pieces[field[0], field[1]];
    }

    private void PlacePieceOnField(Piece piece, int rank, int file)
    {
        int[] field = { rank, file };
        PlacePieceOnField(piece, field);
    }

    public void PlacePieceOnField(Piece piece, int[] field)
    {
        if (piece == null)
        {
            return;
        }
        if (!IsValidField(field))
        {
            throw new InvalidFieldException("Failed to place piece due invalid field.", field);
        }
        if(Pieces[field[0], field[1]] != null)
        {
            throw new FieldAlreadyOccupiedException($"Cannot place {piece.ToString()} on {FieldToString(field)} because the field is occupied by a {Pieces[field[0], field[1]].ToString()}", piece, field);
        }

        // Calculate the position of the piece if the board is at origin with no rotation
        float positionX             = transform.localScale.x * FIELD_SIZE * (0.5f + (field[0] - 4.0f));
        float positionZ             = transform.localScale.z * FIELD_SIZE * (0.5f + (field[1] - 4.0f));
        Vector3 newPosition         = new Vector3(positionX, 0.0f, positionZ);
        // Apply parent transformation so that the piece could be placed with world coordinates
        newPosition                 = transform.rotation *  newPosition;
        newPosition                 = transform.position +  newPosition;
        piece.transform.position    = newPosition;
        Pieces[field[0], field[1]]  = piece;
    }

    public void ClearFieldWithoutDestroying(int[] field)
    {
        if (!IsValidField(field))
        {
            throw new InvalidFieldException($"Failed to clear field due to invalid field.", field);
        }

        Pieces[field[0], field[1]] = null;
    }

    public static bool IsValidField(int rank, int file)
    {
        int[] field = { rank, file };
        return IsValidField(field);
    }

    private static bool IsValidField(int[] field)
    {
        if(field.Length != 2)
        {
            return false;
        }
        return field[0] >= 0 && field[0] <= 7 && field[1] >= 0 && field[1] <= 7;
    }

    private static string FieldToString(int rank, int file)
    {
        int[] field = { rank, file };
        return FieldToString(field);
    }

    private static string FieldToString(int[] field)
    {
        StringBuilder fieldString = new StringBuilder();

        if (!IsValidField(field))
        {
            fieldString.Append("(");
            foreach(int element in field)
            {
                fieldString.Append($"{element}, ");
            }
            fieldString.Remove(fieldString.Length - 3, 1); // Remove ending ", "
            fieldString.Append(")");
            throw new FailedToConvertFieldException($"Failed to convert field {fieldString.ToString()} to string.", field);
        }

        fieldString.Append((char)(65 + field[0]));
        fieldString.Append((char)(48 + field[1] + 1));
        return fieldString.ToString();
    }
}
