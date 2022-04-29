using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public List<Move>   m_Moves;

    public Game(List<Move> game)
    {
        this.m_Moves = game;
    }

    public static Game CreateGame(string pgn)
    {
        // TODO: Create game from pgn
        return null;
    }

    public int GetMoveCount()
    {
        return m_Moves.Count;
    }

    public Move GetMove(int move)
    {
        if(move < GetMoveCount())
        {
            return m_Moves[move];
        }else
        {
            return null;
        }
    }

    override public string ToString()
    {
        return ToString(m_Moves.Count);
    }

    public string ToString(int to)
    {
        string res = "";
        if(to < 1 || to > GetMoveCount())
        {
            // Guard case
        }
        else
        {
            int move = 1;
            for(int i = 0; i < to; ++i)
            {
                res += move + ". ";
                res += m_Moves[i].ToString() + " ";
                ++i;
                if(i < to)
                {
                    res += m_Moves[i].ToString() + " ";
                }
                ++move;
            }
        }
        return res;
    }
}
