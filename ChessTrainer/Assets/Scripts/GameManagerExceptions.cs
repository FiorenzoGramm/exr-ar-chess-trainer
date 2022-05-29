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
    public int[] Field { get; }

    public FailedToClearFieldException(int[] field, Exception innerException = null) : this("", field, innerException)
    { }

    public FailedToClearFieldException(string message, int[] field, Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FailedToMovePieceException : UnityException
{
    public int[] FromField { get; }
    public int[] ToField { get; }

    public FailedToMovePieceException(int[] fromField, int[] toField, Exception innerException = null) : this("", fromField, toField, innerException)
    { }

    public FailedToMovePieceException(string message, int[] fromField, int[] toField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToMoveRookForCastleException : UnityException
{
    public int[] FromField { get; }
    public int[] ToField { get; }

    public FailedToMoveRookForCastleException(int[] fromField, int[] toField, Exception innerException = null) : this("", fromField, toField, innerException)
    { }

    public FailedToMoveRookForCastleException(string message, int[] fromField, int[] toField, Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToPlacePieceException : UnityException
{
    public Piece Piece { get; }
    public int[] Field { get; }

    public FailedToPlacePieceException(Piece piece, int[] field, Exception innerException = null) : this("", piece, field, innerException)
    { }

    public FailedToPlacePieceException(string message, Piece piece, int[] field, Exception innerException = null) : base(message, innerException)
    {
        Piece = piece;
        Field = field;
    }
}
