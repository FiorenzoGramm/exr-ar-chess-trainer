using UnityEngine;

public class InvalidFieldException : UnityException
{
    public InvalidFieldException(string message) : base(message)
    {
    }
}

public class InvalidPieceException : UnityException{
    public InvalidPieceException(string message) : base(message)
    {
    }
}
public class FieldAlreadyOccupiedException : UnityException
{
    public FieldAlreadyOccupiedException(string message) : base(message)
    {
    }
}

public class InvalidGameException : UnityException
{
    public InvalidGameException(string message) : base(message)
    {
    }
}

public class InvalidMoveException : UnityException
{
    public InvalidMoveException(string message) : base(message)
    {
    }
}
