using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board     m_Board;
    public Library   m_Library;
    public Game      m_CurrentGame;
    public UIManager m_UIManager;
    private int      m_CurrentGameIndex;
    public int       m_CurrentMove;
    void Start()
    {

        Library.InitialiseLibrary();
        m_UIManager.InitialiseUI(Library.m_Games);
        changeGame(0);

        // For a better start position (for demo only)
        m_Board.transform.Translate(new Vector3(0.0f, -0.25f, 1.5f), Space.World);
        m_Board.transform.Rotate(new Vector3(0.0f, 90f, 0.0f), Space.World);
    }

    public void Update()
    {
        if (Input.GetKeyUp("left"))
        {
            UndoMove();
            Debug.Log(m_CurrentGame.ToString(m_CurrentMove));
        }
        if (Input.GetKeyUp("right"))
        {
            DoNextMove();
            Debug.Log(m_CurrentGame.ToString(m_CurrentMove));
        }
        if (Input.GetKeyUp("up"))
        {
            changeGame(m_CurrentGameIndex + 1);
        }
        if (Input.GetKeyUp("down"))
        {
            changeGame(m_CurrentGameIndex - 1);
        }
    }
    
    public void changeGame(int index)
    {
        if(index < 0 || index > Library.m_Games.Count - 1)
        {
            Debug.Log("Could not load game with index " + index);
            return;
        }

        m_Board.Reset();
        m_CurrentGame       = Library.m_Games[index];
        m_CurrentGameIndex  = index;
        m_CurrentMove       = 0;
        Debug.Log("Changed game to " + m_CurrentGame.m_Name);
    }

    public void ChangeGame(Game game)
    {
        if (game is null)
        {
            throw new MissingReferenceException("Cannot load game from null pointer");
        }

        m_Board.Reset();
        m_CurrentGame = game;
        m_CurrentGameIndex = 0;
        m_CurrentMove = 0;
        Debug.Log("Changed game to " + m_CurrentGame.m_Name);
    }
    private int[] GetField(string field)
    {
        int[] res = new int[2];
        res[0] = (char)field[0] - 97;
        res[1] = (char)field[1] - 49;
        return res;
    }

    public void DoNextMove()
    {
        if(m_CurrentMove >= m_CurrentGame.GetMoveCount())
        {
            Debug.Log("No moves left.");
            return;
        }

        Move nextMove = m_CurrentGame.GetMove(m_CurrentMove);
        if (nextMove != null)
        {
            int[] from      = nextMove.GetFromFieldCoordinates();
            int[] to        = nextMove.GetToFieldCoordinates();
            Piece toPiece   = m_Board.GetPiece(to[0], to[1]);
            // Check if the move captures anything
            if (toPiece != null)
            {
                // Guard case if the captured piece of the move is not the same as the board (error in move)
                if (!toPiece.m_Symbol.Equals(nextMove.m_ToPiece))
                {
                    Debug.Log(toPiece.ToString());
                    Debug.Log("Pieces are not equal: " + nextMove.m_ToPiece + " (Move) and " + toPiece.m_Symbol + " (Piece).");
                    return;
                }
                m_Board.ClearField(to[0], to[1]);
                Destroy(toPiece.gameObject);
                Debug.Log(m_Board.GetPiece(to[0], to[1]) == null);
            }
            bool res = m_Board.Move(from[0], from[1], to[0], to[1]);
            if (res)
            {
                ++m_CurrentMove;
                if(toPiece != null)
                {
                    Destroy(toPiece.gameObject);
                }
                Debug.Log(m_CurrentGame.ToString(m_CurrentMove));
            }
            else
            {
                Debug.Log("Something went wrong with move: " + nextMove.ToString());
                if (toPiece != null)
                {
                    m_Board.PlacePieceOnField(toPiece, to[0], to[1]);
                }
            }
        }
    }

    public void UndoMove()
    {
        if (m_CurrentMove <= 0)
        {
            Debug.Log("You are already at the start of the game.");
            return;
        }
        
        Move previousMove   = m_CurrentGame.GetMove(m_CurrentMove - 1);
        int[] from          = GetField(previousMove.m_FromField);
        int[] to            = GetField(previousMove.m_ToField);
        Piece toPiece       = null;
        bool res;

        res = m_Board.Move(to[0], to[1], from[0], from[1]);
        if (!res)
        {
            Debug.Log("Something went wrong with undoing move: " + previousMove.ToString());
            return;
        }

        // Check if the move captures anything
        if (!previousMove.m_ToPiece.Equals(""))
        {
            if ((m_CurrentMove - 1) % 2 == 0) // Check if move was by white
            {
                switch (previousMove.m_ToPiece)
                {
                    case "K":
                        toPiece = Instantiate(m_Board.m_BlackKingModel, m_Board.GetComponent<Transform>());
                        break;
                    case "Q":
                        toPiece = Instantiate(m_Board.m_BlackQueenModel, m_Board.GetComponent<Transform>());
                        break;
                    case "R":
                        toPiece = Instantiate(m_Board.m_BlackRookModel, m_Board.GetComponent<Transform>());
                        break;
                    case "B":
                        toPiece = Instantiate(m_Board.m_BlackBishopModel, m_Board.GetComponent<Transform>());
                        break;
                    case "N":
                        toPiece = Instantiate(m_Board.m_BlackKnightModel, m_Board.GetComponent<Transform>());
                        break;
                    case "P":
                        toPiece = Instantiate(m_Board.m_BlackPawnModel, m_Board.GetComponent<Transform>());
                        break;
                    default:
                        Debug.Log("Invalid piece type: " + previousMove.m_ToPiece);
                        break;
                }
            }
            else
            {
                switch (previousMove.m_ToPiece)
                {
                    case "K":
                        toPiece = Instantiate(m_Board.m_WhiteKingModel, m_Board.GetComponent<Transform>());
                        break;
                    case "Q":
                        toPiece = Instantiate(m_Board.m_WhiteQueenModel, m_Board.GetComponent<Transform>());
                        break;
                    case "R":
                        toPiece = Instantiate(m_Board.m_WhiteRookModel, m_Board.GetComponent<Transform>());
                        break;
                    case "B":
                        toPiece = Instantiate(m_Board.m_WhiteBishopModel, m_Board.GetComponent<Transform>());
                        break;
                    case "N":
                        toPiece = Instantiate(m_Board.m_WhiteKnightModel, m_Board.GetComponent<Transform>());
                        break;
                    case "P":
                        toPiece = Instantiate(m_Board.m_WhitePawnModel, m_Board.GetComponent<Transform>());
                        break;
                    default:
                        Debug.Log("Invalid piece type: " + previousMove.m_ToPiece);
                        break;
                }
            }
            res = m_Board.PlacePieceOnField(toPiece, to[0], to[1]);
        }

        if (res)
        {
            --m_CurrentMove;
            Debug.Log(m_CurrentGame.ToString(m_CurrentMove));
        }
        else
        {
            Debug.Log("Failed to place captured piece.");
            if(toPiece != null)
            {
                Destroy(toPiece.gameObject);
            }
            m_Board.Move(from[0], from[1], to[0], to[1]);
        }
        
    }
}
