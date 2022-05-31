using System;
using UnityEngine;

public class MoveExecutionFailureException : UnityException
{
    public Move Move { get; }

    public MoveExecutionFailureException(Move move, Exception innerException = null) : this("", move, innerException)
    { }

    public MoveExecutionFailureException(string message, Move move, Exception innerException = null) : base(message, innerException)
    {
        Move = move;
    }
}
public class FailedToExecuteMoveException : MoveExecutionFailureException
{
    public FailedToExecuteMoveException(Move move, Exception innerException = null) : this("", move, innerException)
    { }

    public FailedToExecuteMoveException(string message, Move move, Exception innerException = null) :base(message, move, innerException)
    { }
}

public class FailedToUndoMoveException : MoveExecutionFailureException
{
    public FailedToUndoMoveException(Move move, Exception innerException) : base("", move, innerException)
    { }

    public FailedToUndoMoveException(string message, Move move, Exception innerException) : base(message, move, innerException)
    { }
}

public class FailedToClearFieldException : UnityException
{
    public Vector2Int Field { get; }

    public FailedToClearFieldException(Vector2Int field, Exception innerException = null) : this("", field, innerException)
    { }

    public FailedToClearFieldException(string message, Vector2Int field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FailedToMovePieceException : UnityException
{
    public Vector2Int FromField { get; }
    public Vector2Int ToField { get; }

    public FailedToMovePieceException(Vector2Int fromField, Vector2Int toField, Exception innerException = null) : this("", fromField, toField, innerException)
    { }

    public FailedToMovePieceException(string message, Vector2Int fromField, Vector2Int toField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToMoveRookForCastleException : UnityException
{
    public Vector2Int FromField { get; }
    public Vector2Int ToField { get; }

    public FailedToMoveRookForCastleException(Vector2Int fromField, Vector2Int toField, Exception innerException = null) : this("", fromField, toField, innerException)
    { }

    public FailedToMoveRookForCastleException(string message, Vector2Int fromField, Vector2Int toField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToPlacePieceException : UnityException
{
    public Piece Piece { get; }
    public Vector2Int Field { get; }

    public FailedToPlacePieceException(Piece piece, Vector2Int field, Exception innerException = null) : this("", piece, field, innerException)
    { }

    public FailedToPlacePieceException(string message, Piece piece, Vector2Int field, Exception innerException = null) : base(message, innerException)
    {
        Piece = piece;
        Field = field;
    }
}
public class FailedToGetPieceException : UnityException
{
    public Vector2Int Field { get; }

    public FailedToGetPieceException(Vector2Int field, Exception innerException = null) : this("", field, innerException)
    { }

    public FailedToGetPieceException(string message, Vector2Int field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}
