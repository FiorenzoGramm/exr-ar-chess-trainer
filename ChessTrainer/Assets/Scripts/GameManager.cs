using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board    m_Board;
    public Game     m_Game;
    public int      m_CurrentMove;

    void Start()
    {
        // Test - Start
        List<Move> game = new List<Move>();
        game.Add(new Move("e2", "e4"));
        game.Add(new Move("c7", "c6"));
        game.Add(new Move("d2", "d4"));
        game.Add(new Move("d7", "d5"));
        game.Add(new Move("e4", "e5"));
        game.Add(new Move("B", "c8", "f5"));
        game.Add(new Move("B", "f1", "d3"));
        game.Add(new Move("B", "f5", "B", "d3"));
        this.m_Game = new Game(game);
        // Test - End

        m_Board.Reset();
    }

    public void Update()
    {
        int tmp = m_CurrentMove;
        if (Input.GetKeyUp("left"))
        {
            UndoMove();
        }
        if (Input.GetKeyUp("right"))
        {
            DoNextMove();
        }

        if (m_CurrentMove != tmp)
        {
            Debug.Log(m_Game.ToString(m_CurrentMove));
        }
    }

    private int[] GetField(string field)
    {
        int[] res = new int[2];
        res[0] = (char)field[0] - 97;
        res[1] = (char)field[1] - 49;
        return res;
    }

    private void DoNextMove()
    {
        if(m_CurrentMove < m_Game.GetMoveCount())
        {
            Move nextMove = m_Game.GetMove(m_CurrentMove);
            if (nextMove != null)
            {
                int[] from      = GetField(nextMove.m_FromField);
                int[] to        = GetField(nextMove.m_ToField);
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
                    m_Board.m_Pieces[to[0], to[1]] = null;
                    Destroy(toPiece);
                }
                bool res = m_Board.Move(from[0], from[1], to[0], to[1]);
                if (res)
                {
                    ++m_CurrentMove;
                    if(toPiece != null)
                    {
                        Debug.Log("Destroy piece " + toPiece.m_Type);
                        Destroy(toPiece.gameObject);
                    }
                    
                }
                else
                {
                    Debug.Log("Something went wrong with move: " + nextMove.ToString());
                    if (toPiece != null)
                    {
                        m_Board.m_Pieces[to[0], to[1]] = toPiece;
                    }
                }
            }
        }
        else
        {
            Debug.Log("No moves left.")
;       }
    }

    private void UndoMove()
    {
        if(m_CurrentMove > 0)
        {
            Move previousMove   = m_Game.GetMove(m_CurrentMove - 1);
            int[] from          = GetField(previousMove.m_FromField);
            int[] to            = GetField(previousMove.m_ToField);
            Piece toPiece       = null;
            bool res            = true;
            // Check if the move captures anything
            if (!previousMove.m_ToPiece.Equals(""))
            {
                if ((m_CurrentMove - 1) % 2 == 0)
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
            }
            
            res = m_Board.Move(to[0], to[1], from[0], from[1]);
            if (res)
            {
                if (!(toPiece is null))
                {
                    Debug.Log("Try to place " + toPiece.ToString() + " on " + Board.FieldToString(to[0], to[1]));
                    res = m_Board.PlacePieceOnField(toPiece, to[0], to[1]);
                }
                if (res)
                {
                    --m_CurrentMove;
                }
                else
                {
                    Debug.Log("Something went wrong with undoing move: " + previousMove.ToString());
                    m_Board.m_Pieces[to[0], to[1]] = null;
                    m_Board.Move(from[0], from[1], to[0], to[1]);
                }
            }
            else
            {
                Debug.Log("Failed to place " + toPiece.ToString() + " on " + Board.FieldToString(to[0], to[1]));
            }
        }
        else
        {
            Debug.Log("Already at the start of the game.");
        }
    }
}
