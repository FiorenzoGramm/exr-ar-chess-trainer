using UnityEngine;

public class Board : MonoBehaviour
{
    // White pieces (prefabs)
    public Piece m_WhitePawnModel;
    public Piece m_WhiteKingModel;
    public Piece m_WhiteQueenModel;
    public Piece m_WhiteBishopModel;
    public Piece m_WhiteKnightModel;
    public Piece m_WhiteRookModel;
    // Black pieces (prefabs)
    public Piece m_BlackPawnModel;
    public Piece m_BlackKingModel;
    public Piece m_BlackQueenModel;
    public Piece m_BlackBishopModel;
    public Piece m_BlackKnightModel;
    public Piece m_BlackRookModel;

    public Piece   [,] m_Pieces         = new Piece[8,8];

    private const float FIELD_SIZE      = 0.06f / 100f; // For correct movment of the pieces

    public void Reset()
    {
        // Clear current field
        for(int rank = 0; rank < 8; ++rank)
        {
            for(int file = 0; file < 8; ++file)
            {
                if(m_Pieces[rank, file] != null)
                {
                    Destroy(m_Pieces[rank, file].gameObject);
                }
                m_Pieces[rank, file] = null;
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
                Piece p = m_Pieces[rank, file];
                if(p == null)
                {
                    Debug.Log((char)(rank + 65) + "" + (file + 1) + ":");
                }
                else
                {
                    if (p.m_IsWhite)
                    {
                        Debug.Log((char)(rank + 65) + "" + (file + 1) + ": White " + p.m_Type);
                    }
                    else
                    {
                        Debug.Log((char)(rank + 65) + "" + (file + 1) + ": Black " + p.m_Type);
                    }
                }
            }
        }
    }

    private void SpawnPawns()
    {
        for (int i = 0; i < 8; ++i)
        {
            Piece whitePawn = Instantiate(m_WhitePawnModel, this.transform);
            Piece blackPawn = Instantiate(m_BlackPawnModel, this.transform);

            PlacePieceOnField(whitePawn, i, 1);
            PlacePieceOnField(blackPawn, i, 6);
        }
    }

    private void SpawnKings()
    {
        Piece whiteKing = Instantiate(m_WhiteKingModel, this.transform);
        Piece blackKing = Instantiate(m_BlackKingModel, this.transform);

        PlacePieceOnField(whiteKing, 4 ,0);
        PlacePieceOnField(blackKing, 4 ,7);
    }

    private void SpawnQueens()
    {
        Piece whiteQueen = Instantiate(m_WhiteQueenModel, this.transform);
        Piece blackQueen = Instantiate(m_BlackQueenModel, this.transform);

        PlacePieceOnField(whiteQueen, 3, 0);
        PlacePieceOnField(blackQueen, 3, 7);
    }

    private void SpawnBishops()
    {
        Piece whiteLeftBishop   = Instantiate(m_WhiteBishopModel, this.transform);
        Piece whiteRightBishop  = Instantiate(m_WhiteBishopModel, this.transform);
        Piece blackLeftBishop   = Instantiate(m_BlackBishopModel, this.transform);
        Piece blackRightBishop  = Instantiate(m_BlackBishopModel, this.transform);

        PlacePieceOnField(whiteLeftBishop,  2, 0);
        PlacePieceOnField(whiteRightBishop, 5, 0);
        PlacePieceOnField(blackLeftBishop,  2, 7);
        PlacePieceOnField(blackRightBishop, 5, 7);
    }

    private void SpawnKnights()
    {
        Piece whiteLeftKnight   = Instantiate(m_WhiteKnightModel, this.transform);
        Piece whiteRightKnight  = Instantiate(m_WhiteKnightModel, this.transform);
        Piece blackLeftKnight   = Instantiate(m_BlackKnightModel, this.transform);
        Piece blackRightKnight  = Instantiate(m_BlackKnightModel, this.transform);

        PlacePieceOnField(whiteLeftKnight,  1, 0);
        PlacePieceOnField(whiteRightKnight, 6, 0);
        PlacePieceOnField(blackLeftKnight,  1, 7);
        PlacePieceOnField(blackRightKnight, 6, 7);
    }

    private void SpawnRooks()
    {
        Piece whiteLeftRook     = Instantiate(m_WhiteRookModel, this.transform);
        Piece whiteRightRook    = Instantiate(m_WhiteRookModel, this.transform);
        Piece blackLeftRook     = Instantiate(m_BlackRookModel, this.transform);
        Piece blackRightRook    = Instantiate(m_BlackRookModel, this.transform);

        PlacePieceOnField(whiteLeftRook,    0, 0);
        PlacePieceOnField(whiteRightRook,   7, 0);
        PlacePieceOnField(blackLeftRook,    0, 7);
        PlacePieceOnField(blackRightRook,   7, 7);
    }

    public bool Move(int fromRank, int fromFile, int toRank, int toFile)
    {
        Piece fromPiece = m_Pieces[fromRank, fromFile];
        Piece toPiece   = m_Pieces[toRank, toFile];

        if(fromPiece == null)
        {
            Debug.Log("There is no piece on " + FieldToString(fromRank, fromFile) + " to move to " + FieldToString(toRank, toFile));
            return false;
        }

        if(toPiece != null)
        {
            Debug.Log("There is a piece on " + FieldToString(fromRank, fromFile) + "(" + toPiece.ToString() + ")");
            return false;
        }

        bool piecePlacedSuccessfully = PlacePieceOnField(fromPiece, toRank, toFile);

        if (piecePlacedSuccessfully)
        {
            m_Pieces[fromRank, fromFile] = null;
        }

        return piecePlacedSuccessfully;
    }

    public Piece GetPiece(int rank, int file)
    {
        if (!IsValidField(rank, file))
        {
            return null;
        }
        return m_Pieces[rank, file];
    }

    public bool PlacePieceOnField(Piece piece, int rank, int file)
    {
        if (piece is null)
        {
            Debug.Log("Cannot place a piece on field " + FieldToString(rank, file) + "(piece is null)");
            return false;
        }
        if (!IsValidField(rank, file))
        {
            Debug.Log(FieldToString(rank, file) + "(" + rank + "," + file + ") is an invalid field for placing a piece");
            return false;
        }
        if(m_Pieces[rank, file] != null)
        {
            Debug.Log("Cannot place " + piece.ToString() + " on " + FieldToString(rank, file) + " because the field is occupied by a " + m_Pieces[rank, file].ToString());
            return false;
        }

        // Calculate the position of the piece if the board is at origin with no rotation
        float positionX             = transform.localScale.x * FIELD_SIZE * (0.5f + (rank - 4.0f));
        float positionZ             = transform.localScale.z * FIELD_SIZE * (0.5f + (file - 4.0f));
        Vector3 newPosition         = new Vector3(positionX, 0.0f, positionZ);
        // Apply parent transformatin so that the piece could be placed with world coordinates
        newPosition                 = transform.rotation *  newPosition;
        newPosition                 = transform.position +  newPosition;
        piece.transform.position    = newPosition;
        m_Pieces[rank, file]        = piece;
        return true;
    }
    public bool ClearField(int rank, int file)
    {
        if(!IsValidField(rank, file))
        {
            Debug.Log("Failed to clear field because [" + rank + ", " + file + "] is an invalid field.");
            return false;
        }

        Piece piece = GetPiece(rank, file);
        if(piece == null)
        {
            Debug.Log("Failed to clear " + FieldToString(rank, file) + " is alreade empty.");
            return false;
        }

        m_Pieces[rank, file] = null;
        return true;
    }

    public static bool IsValidField(int[] position)
    {
        if(position.Length == 2)
        {
            return IsValidField(position[0], position[1]);
        }
        else
        {
            return false;
        }
       
    }

    public static bool IsValidField(int rank, int file)
    {
        return (rank >= 0 && rank <= 7 && file >= 0 && file <= 7);
    }

    public static string FieldToString(int rank, int file)
    {
        string res = "";
        if (!IsValidField(rank, file))
        {
            return res;
        }
        res += (char)(65 + rank);
        res += (char)(48 + file + 1);
        return res;
    }
}
