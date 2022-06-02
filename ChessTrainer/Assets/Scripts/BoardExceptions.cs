using System;
using UnityEngine;

public class InvalidFieldException : UnityException
{
    public Vector2Int Field { get; }

    public InvalidFieldException(Vector2Int field, Exception innerException = null) : this("", field, innerException)
    { }

    public InvalidFieldException(string message, Vector2Int field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FieldAlreadyOccupiedException : UnityException
{
    public Piece FromPiece { get; }
    public Field ToField { get; }

    public FieldAlreadyOccupiedException(Piece fromPiece, Field toField, Exception innerException = null)
        : this("", fromPiece, toField, innerException)
    { }

    public FieldAlreadyOccupiedException(string message, Piece fromPiece, Field toField, Exception innerException = null) 
        : base(message, innerException)
    {
        FromPiece = fromPiece;
        ToField = toField;
    }
}

public class ThereIsNoPieceException : UnityException
{
    public Field FromField { get; }

    public ThereIsNoPieceException(Field fromField, Exception innerException = null) : this("", fromField, innerException)
    { }

    public ThereIsNoPieceException(string message, Field fromField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
    }
}

