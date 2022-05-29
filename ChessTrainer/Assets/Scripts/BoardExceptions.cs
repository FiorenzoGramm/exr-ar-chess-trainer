using System;
using UnityEngine;

public class InvalidFieldException : UnityException
{
    public int[] Field { get; }

    public InvalidFieldException(int[] field, Exception innerException = null) : this("", field, innerException)
    { }

    public InvalidFieldException(string message, int[] field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FailedToConvertFieldException : UnityException
{
    public int[] Field { get; }

    public FailedToConvertFieldException(int[] field, Exception innerException = null) : this("", field, innerException)
    { }

    public FailedToConvertFieldException(string message, int[] field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FieldAlreadyOccupiedException : UnityException
{
    public Piece FromPiece { get; }
    public int[] ToField { get; }

    public FieldAlreadyOccupiedException(Piece fromPiece, int[] toField, Exception innerException = null)
        : this("", fromPiece, toField, innerException)
    { }

    public FieldAlreadyOccupiedException(string message, Piece fromPiece, int[] toField, Exception innerException = null) 
        : base(message, innerException)
    {
        FromPiece = fromPiece;
        ToField = toField;
    }
}

public class ThereIsNoPieceException : UnityException
{
    public int[] FromField { get; }

    public ThereIsNoPieceException(int[] fromField, Exception innerException = null) : this("", fromField, innerException)
    { }

    public ThereIsNoPieceException(string message, int[] fromField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
    }
}

