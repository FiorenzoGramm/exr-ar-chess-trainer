using System;
using UnityEngine;

public class InvalidGameIndexException : UnityException
{
    public int GameIndex { get; }

    public InvalidGameIndexException(int index, string message = "", Exception innerException = null) : base(message, innerException)
    {
        GameIndex = index;
    }
}