using System;
using UnityEngine;

public class MoveExecutionFailureException : UnityException
{
    public Move Move { get; }

    public MoveExecutionFailureException(Move move, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Move = move;
    }
}
public class FailedToExecuteMoveException : MoveExecutionFailureException
{
    public FailedToExecuteMoveException(Move move, string message = "", Exception innerException = null) :base(move, message, innerException)
    { }
}

public class FailedToUndoMoveException : MoveExecutionFailureException
{
    public FailedToUndoMoveException(Move move, string message, Exception innerException) : base(move, message, innerException)
    { }
}

public class FailedToClearFieldException : UnityException
{
    public int[] Field { get; }

    public FailedToClearFieldException(int[] field, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Field = field;
    }
}

public class FailedToMovePieceException : UnityException
{
    public int[] FromField { get; }
    public int[] ToField { get; }

    public FailedToMovePieceException(int[] fromField, int[] toField, string message = "", Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToMoveRookForCastleException : UnityException
{
    public int[] FromField { get; }
    public int[] ToField { get; }

    public FailedToMoveRookForCastleException(int[] fromField, int[] toField, string message = "", Exception innerException = null) : base(message, innerException)
    {
        FromField = fromField;
        ToField = toField;
    }
}

public class FailedToPlacePieceException : UnityException
{
    public Piece Piece { get; }
    public int[] Field { get; }

    public FailedToPlacePieceException(Piece piece, int[] field, string message = "", Exception innerException = null) : base(message, innerException)
    {
        Piece = piece;
        Field = field;
    }
}
