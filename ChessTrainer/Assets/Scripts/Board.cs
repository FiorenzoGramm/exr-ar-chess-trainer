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

    private const float FIELD_SIZE      = 0.06f / 100f;
    public Vector3      ORIGNAL_ROTATION;

    public void Start()
    {
        ORIGNAL_ROTATION = transform.rotation.eulerAngles;
    }

    public void Reset()
    {
        // Sets the transform to a standard value for placing the pieces independendly from the current tranform
        Vector3     oldPosition     = this.transform.position;
        Quaternion  oldRotation     = this.transform.rotation;
        Vector3     oldScale        = this.transform.localScale;
        this.transform.position     = new Vector3(0, 0, 0);
        this.transform.rotation     = Quaternion.Euler(-90, 0, 0);
        this.transform.localScale   = new Vector3(100, 100, 100);

        // Clear current field
        for(int rank = 0; rank < 8; ++rank)
        {
            for(int file = 0; file < 8; ++file)
            {
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

        // Reset transform
        this.transform.position     = oldPosition;
        this.transform.rotation     = oldRotation;
        this.transform.localScale   = oldScale;
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

            float positionX      = (i * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
            float positionZWhite = (1 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
            float positionZBlack = (6 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
            Vector3 positionWhite = new Vector3(positionX, 0, positionZWhite);
            Vector3 positionBlack = new Vector3(positionX, 0, positionZBlack);
            whitePawn.transform.position += positionWhite;
            blackPawn.transform.position += positionBlack;

            m_Pieces[i, 1] = whitePawn;
            m_Pieces[i, 6] = blackPawn;
        }
    }

    private void SpawnKings()
    {
        Piece whiteKing = Instantiate(m_WhiteKingModel, this.transform);
        Piece blackKing = Instantiate(m_BlackKingModel, this.transform);

        float positionX         = (4 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionZWhite    = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        float positionZBlack    = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        Vector3 positionWhite = new Vector3(positionX, 0, positionZWhite);
        Vector3 positionBlack = new Vector3(positionX, 0, positionZBlack);
        whiteKing.transform.position += positionWhite;
        blackKing.transform.position += positionBlack;

        m_Pieces[4, 0] = whiteKing;
        m_Pieces[4, 7] = blackKing;
    }

    private void SpawnQueens()
    {
        Piece whiteQueen = Instantiate(m_WhiteQueenModel, this.transform);
        Piece blackQueen = Instantiate(m_BlackQueenModel, this.transform);

        float positionX      = (3 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionZWhite = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        float positionZBlack = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        Vector3 positionWhite = new Vector3(positionX, 0, positionZWhite);
        Vector3 positionBlack = new Vector3(positionX, 0, positionZBlack);
        whiteQueen.transform.position += positionWhite;
        blackQueen.transform.position += positionBlack;

        m_Pieces[3, 0] = whiteQueen;
        m_Pieces[3, 7] = blackQueen;
    }

    private void SpawnBishops()
    {
        Piece whiteLeftBishop   = Instantiate(m_WhiteBishopModel, this.transform);
        Piece whiteRightBishop  = Instantiate(m_WhiteBishopModel, this.transform);
        Piece blackLeftBishop   = Instantiate(m_BlackBishopModel, this.transform);
        Piece blackRightBishop  = Instantiate(m_BlackBishopModel, this.transform);

        float positionXLeft     = (2 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionXRight    = (5 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionZWhite    = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        float positionZBlack    = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        Vector3 positionWhiteLeft   = new Vector3(positionXLeft,  0, positionZWhite);
        Vector3 positionWhiteRight  = new Vector3(positionXRight, 0, positionZWhite);
        Vector3 positionBlackLeft   = new Vector3(positionXLeft,  0, positionZBlack);
        Vector3 positionBlackRight  = new Vector3(positionXRight, 0, positionZBlack);
        whiteLeftBishop.transform.position  += positionWhiteLeft;
        whiteRightBishop.transform.position += positionWhiteRight;
        blackLeftBishop.transform.position  += positionBlackLeft;
        blackRightBishop.transform.position += positionBlackRight;

        m_Pieces[2, 0] = whiteLeftBishop;
        m_Pieces[5, 0] = whiteRightBishop;
        m_Pieces[2, 7] = blackLeftBishop;
        m_Pieces[5, 7] = blackRightBishop;
    }

    private void SpawnKnights()
    {
        Piece whiteLeftKnight   = Instantiate(m_WhiteKnightModel, this.transform);
        Piece whiteRightKnight  = Instantiate(m_WhiteKnightModel, this.transform);
        Piece blackLeftKnight   = Instantiate(m_BlackKnightModel, this.transform);
        Piece blackRightKnight  = Instantiate(m_BlackKnightModel, this.transform);

        float positionXLeft     = (1 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionXRight    = (6 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionZWhite    = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        float positionZBlack    = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        Vector3 positionWhiteLeft   = new Vector3(positionXLeft,  0, positionZWhite);
        Vector3 positionWhiteRight  = new Vector3(positionXRight, 0, positionZWhite);
        Vector3 positionBlackLeft   = new Vector3(positionXLeft,  0, positionZBlack);
        Vector3 positionBlackRight  = new Vector3(positionXRight, 0, positionZBlack);
        whiteLeftKnight.transform.position  += positionWhiteLeft;
        whiteRightKnight.transform.position += positionWhiteRight;
        blackLeftKnight.transform.position  += positionBlackLeft;
        blackRightKnight.transform.position += positionBlackRight;

        m_Pieces[1, 0] = whiteLeftKnight;
        m_Pieces[6, 0] = whiteRightKnight;
        m_Pieces[1, 7] = blackLeftKnight;
        m_Pieces[6, 7] = blackRightKnight;
    }

    private void SpawnRooks()
    {
        Piece whiteLeftRook     = Instantiate(m_WhiteRookModel, this.transform);
        Piece whiteRightRook    = Instantiate(m_WhiteRookModel, this.transform);
        Piece blackLeftRook     = Instantiate(m_BlackRookModel, this.transform);
        Piece blackRightRook    = Instantiate(m_BlackRookModel, this.transform);

        float positionXLeft  = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionXRight = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.x;
        float positionZWhite = (0 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        float positionZBlack = (7 * FIELD_SIZE + 0.5f * FIELD_SIZE - 4 * FIELD_SIZE) * this.transform.localScale.y;
        Vector3 positionWhiteLeft   = new Vector3(positionXLeft,  0, positionZWhite);
        Vector3 positionWhiteRight  = new Vector3(positionXRight, 0, positionZWhite);
        Vector3 positionBlackLeft   = new Vector3(positionXLeft,  0, positionZBlack);
        Vector3 positionBlackRight  = new Vector3(positionXRight, 0, positionZBlack);
        whiteLeftRook.transform.position    += positionWhiteLeft;
        whiteRightRook.transform.position   += positionWhiteRight;
        blackLeftRook.transform.position    += positionBlackLeft;
        blackRightRook.transform.position   += positionBlackRight;

        m_Pieces[0, 0] = whiteLeftRook;
        m_Pieces[7, 0] = whiteRightRook;
        m_Pieces[0, 7] = blackLeftRook;
        m_Pieces[7, 7] = blackRightRook;
    }

    public bool Move(int fromRank, int fromFile, int toRank, int toFile)
    {
        Piece fromPiece = m_Pieces[fromRank, fromFile];
        Piece toPiece   = m_Pieces[toRank, toFile];
        
        if(toPiece == null)
        {
            m_Pieces[toRank, toFile] = fromPiece;
            m_Pieces[fromRank, fromFile] = null;

            int movementX = toRank - fromRank;
            int movementZ = toFile - fromFile;

            TranslatePiece(fromPiece, new Vector3(FIELD_SIZE * this.transform.localScale.x * movementX, 0.0f, FIELD_SIZE * this.transform.localScale.y * movementZ));
            return true;
        }
        else
        {
            Debug.Log(FieldToString(fromRank, fromFile) + " -> " + FieldToString(toRank, toFile)+ " is a invalid move, field is occupied.");
            return false;
        }
        
    }

    public Piece GetPiece(int rank, int file)
    {
        if(rank >= 0 && rank <= 7 && file >= 0 && file <= 7)
        {
            return m_Pieces[rank, file];
        }
        else
        {
            return null;
        }
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
            Debug.Log(FieldToString(rank, file) + " is an invalid field for placing a piece");
            return false;
        }
        if(m_Pieces[rank, file] != null)
        {
            Debug.Log("Cannot place " + piece.ToString() + " on " + FieldToString(rank, file) + " because the field is occupied by a " + m_Pieces[rank, file].ToString());
            return false;
        }
        // TODO: Create coorect movement, even with switched coordinates
        m_Pieces[rank, file] = piece;
        float positionX = this.GetComponent<Transform>().localScale.x * FIELD_SIZE * (0.5f + (rank - 4.0f));
        float positionZ = this.GetComponent<Transform>().localScale.z * FIELD_SIZE * (0.5f + (file - 4.0f));
        TranslatePiece(piece, new Vector3(positionX, 0.0f, positionZ));
        Debug.Log("Placed " + piece.ToString() + " on " + FieldToString(rank, file));
        return true;
    }

    private void TranslatePiece(Piece piece, Vector3 direction)
    {
        direction  = Quaternion.Euler(-ORIGNAL_ROTATION) * direction;

        piece.GetComponent<Transform>().Translate(direction, Space.Self);
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
