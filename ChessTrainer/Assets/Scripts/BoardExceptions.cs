using System;
using UnityEngine;

public class InvalidFieldException : UnityException
{
    public int[] Field { get; }
    public InvalidFieldException(int[] field, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FailedToConvertFieldException : UnityException
{
    public string Field { get; }
    public FailedToConvertFieldException(string field, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FieldAlreadyOccupiedException : UnityException
{
    public Piece FromPiece { get; }
    public int[] ToField { get; }

    public FieldAlreadyOccupiedException(Piece fromPiece, int[] toField, string message = "", Exception innerException = null) 
        : base(message, innerException)
    {
        FromPiece = fromPiece;
        ToField = toField;
    }
}

public class ThereIsNoPieceException : UnityException
{
    public int[] FromField { get; }
    public ThereIsNoPieceException(int[] fromField, string message = "", Exception innerException = null) 
        : base(message, innerException)
    {
        FromField = fromField;
    }
}

