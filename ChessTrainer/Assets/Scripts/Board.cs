using System;
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

    public Piece[,] Pieces = new Piece[8,8];

    public const float FIELD_SIZE = 0.06f / 100f; // For correct movment of the pieces

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
        for (int rank = 0; rank < 8; ++rank)
        {
            for (int file = 0; file < 8; ++file)
            {
                Piece currentPiece = Pieces[rank, file];
                StringBuilder fieldString = new StringBuilder();

                fieldString.Append($"{FieldToString(rank, file)} : ");

                if (currentPiece == null)
                {
                    fieldString.Append($"-");
                }
                else
                {
                    fieldString.Append(currentPiece.ToString());
                }
                Debug.Log(fieldString.ToString());
            }
        }
    }

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

    internal static bool IsValidField(string fromField)
    {
        throw new NotImplementedException();
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

    public void Move(int fromRank, int fromFile, int toRank, int toFile)
    {
        if(!IsValidField(fromRank, fromFile) || !IsValidField(toRank, toFile))
        {
            throw new InvalidFieldException($"Cannot do move due invalid fields: from ({fromRank} ,{fromFile}) or to ({toRank} ,{toFile})");
        }

        Piece fromPiece = Pieces[fromRank, fromFile];
        Piece toPiece   = Pieces[toRank, toFile];

        if(fromPiece == null)
        {
            throw new InvalidMoveException($"There is no piece on {FieldToString(fromRank, fromFile)} to move to {FieldToString(toRank, toFile)}");
        }

        if(toPiece != null)
        {
            throw new FieldAlreadyOccupiedException($"There is a piece on {FieldToString(toRank, toFile)} ({toPiece.ToString()})");
        }

        try
        {
            PlacePieceOnField(fromPiece, toRank, toFile);
            Pieces[fromRank, fromFile] = null;
        } catch (UnityException exception)
        {
            Debug.LogError(exception);
        }
    }

    public Piece GetPiece(int rank, int file)
    {
        if (!IsValidField(rank, file))
        {
            return null;
        }
        return Pieces[rank, file];
    }

    public void PlacePieceOnField(Piece piece, int rank, int file)
    {
        if (piece == null)
        {
            return;
        }
        if (!IsValidField(rank, file))
        {
            throw new InvalidFieldException($"{FieldToString(rank, file)} ({rank} ,{file}) is an invalid field for placing a piece");
        }
        if(Pieces[rank, file] != null)
        {
            throw new FieldAlreadyOccupiedException($"Cannot place {piece.ToString()} on {FieldToString(rank, file)} because the field is occupied by a {Pieces[rank, file].ToString()}");
        }

        // Calculate the position of the piece if the board is at origin with no rotation
        float positionX             = transform.localScale.x * FIELD_SIZE * (0.5f + (rank - 4.0f));
        float positionZ             = transform.localScale.z * FIELD_SIZE * (0.5f + (file - 4.0f));
        Vector3 newPosition         = new Vector3(positionX, 0.0f, positionZ);
        // Apply parent transformation so that the piece could be placed with world coordinates
        newPosition                 = transform.rotation *  newPosition;
        newPosition                 = transform.position +  newPosition;
        piece.transform.position    = newPosition;
        Pieces[rank, file]        = piece;
    }
    public void ClearFieldWithoutDestroying(int rank, int file)
    {
        if(!IsValidField(rank, file))
        {
            throw new InvalidFieldException($"Failed to clear field because [{rank}, {file}] is an invalid field.");
        }

        Pieces[rank, file] = null;
    }

    public static bool IsValidField(int rank, int file)
    {
        return rank >= 0 && rank <= 7 && file >= 0 && file <= 7;
    }

    public static string FieldToString(int rank, int file)
    {
        StringBuilder fieldString = new StringBuilder();
        if (!IsValidField(rank, file))
        {
            throw new InvalidFieldException($"({rank}, {file}) is not a valid field.");
        }
        fieldString.Append((char)(65 + rank));
        fieldString.Append((char)(48 + file + 1));
        return fieldString.ToString();
    }

    public static int[] StringToField(string fieldString)
    {
        if(fieldString.Length < 2)
        {
            throw new InvalidFieldException($"Cannot get a field from string: {fieldString}");
        }

        int[] field = new int[2];
        field[0] = (char)fieldString[0] - 97;
        field[1] = (char)fieldString[1] - 49;

        if(!IsValidField(field[0], field[1]))
        {
            throw new InvalidFieldException($"{fieldString} is not a valid field.");
        }

        return field;
    }
}
