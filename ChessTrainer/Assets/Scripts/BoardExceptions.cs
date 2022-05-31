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

public class FailedToConvertFieldException : UnityException
{
    public Vector2Int Field { get; }

    public FailedToConvertFieldException(Vector2Int field, Exception innerException = null) : this("", field, innerException)
    { }

    public FailedToConvertFieldException(string message, Vector2Int field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FieldAlreadyOccupiedException : UnityException
{
    public Piece FromPiece { get; }
    public Vector2Int ToField { get; }

    public FieldAlreadyOccupiedException(Piece fromPiece, Vector2Int toField, Exception innerException = null)
        : this("", fromPiece, toField, innerException)
    { }

    public FieldAlreadyOccupiedException(string message, Piece fromPiece, Vector2Int toField, Exception innerException = null) 
        : base(message, innerException)
    {
        FromPiece = fromPiece;
        ToField = toField;
    }
}

public class ThereIsNoPieceException : UnityException
{
    public Vector2Int FromField { get; }

    public ThereIsNoPieceException(Vector2Int fromField, Exception innerException = null) : this("", fromField, innerException)
    { }

    public ThereIsNoPieceException(string message, Vector2Int fromField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
    }
}

