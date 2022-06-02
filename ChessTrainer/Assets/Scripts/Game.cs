using System;
using System.Collections.Generic;
using System.Text;

public class Game
{
    public string       name;
    public List<Move>   moves;

    public Game(string name) 
        : this(name, new List<Move>())
    {}

    public Game(string name, List<Move> moves)
    {
        this.name = name;
        this.moves = moves;
    }

    public int GetMoveCount()
    {
        return moves.Count;
    }

    public Move GetMove(int move)
    {
        if(move >= 0 && move < GetMoveCount())
        {
            return moves[move];
        }
        else
        {
            return null;
        }
    }

    override public string ToString()
    {
        return ToString(moves.Count);
    }

    public string ToString(int to)
    {
        StringBuilder gameString = new StringBuilder();
        if(to < 1 || to > GetMoveCount())
        {
            return gameString.ToString();
        }

        to = Math.Min(to, GetMoveCount()); // If 'to' is greather than the move count, the whole game is returned as string

        int moveNumberIndex = 1;
        for (int i = 0; i < to; ++i)
        {
            gameString.Append($"{moveNumberIndex}. ");
            gameString.Append($"{moves[i].ToString()} ");
            ++i;
            if (i < to)
            {
                gameString.Append($"{moves[i].ToString()} ");
            }
            ++moveNumberIndex;
        }
        return gameString.ToString();
    }
}
