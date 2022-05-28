using System;
using UnityEngine;

public class  InvalidMoveIndexException : UnityException
{
    public int MoveIndex { get; }

    public InvalidMoveIndexException(int index, Exception innerException) : this("", index, innerException)
    { }

    public InvalidMoveIndexException(string message, int index, Exception innerException) : base(message, innerException)
    {
        MoveIndex = index;
    }
}
