using System;
using UnityEngine;

public class Move
{
    private char        fromPiece;
    private Vector2Int  fromField;
    private char        toPiece;
    private Vector2Int  toField;
    private char        promotion;
    private bool        check;
    private bool        mate;
    private string      shortAlgebraicNotation;

    #region Object construction
    private Move() 
    {
        fromPiece = '\0';
        fromField = new Vector2Int(-1, -1);
        toPiece = '\0';
        toField = new Vector2Int(-1, -1);
        promotion = '\0';
        check = false;
        mate = false;
        shortAlgebraicNotation = "NULL_MOVE";
    }

    public static Move CreateSimplePawnMove(string fromField, string toField)
    {
        return CreateFullQualifiedMove('P', fromField, ' ', toField, ' ', false, false, toField);
    }

    public static Move CreateSimplePieceMove(char fromPiece, string fromField, string toField)
    {
        Move newMove = CreateFullQualifiedMove(fromPiece, fromField, ' ', toField, ' ', false, false, "");

        if (newMove == null)
        {
            return newMove;
        }

        newMove.shortAlgebraicNotation = $"{fromPiece}{toField}";
        return newMove;
    }

    public static Move CreateSimplePieceCapture(char fromPiece, string fromField, char toPiece, string toField)
    {
        Move newMove = CreateFullQualifiedMove(fromPiece, fromField, toPiece, toField, ' ', false, false, "");

        if (newMove == null)
        {
            return newMove;
        }

        newMove.shortAlgebraicNotation = $"{fromPiece}x{toField}";
        return newMove;
    }

    public static Move CreateSimplePawnCapture(string fromField, char toPiece, string toField)
    {
        Move newMove = CreateFullQualifiedMove('P', fromField, toPiece, toField, ' ', false, false, "");

        if(newMove == null)
        {
            return newMove;
        }

        newMove.shortAlgebraicNotation = $"{fromField[0]}x{toField}";
        return newMove;
    }

    public static Move CreateNonCheckMove(char fromPiece, string fromField, char toPiece, string toField, char promotion, string shortAlgebraicNotation)
    {
        return CreateFullQualifiedMove(fromPiece, fromField, toPiece, toField, promotion, false, false, shortAlgebraicNotation);
    }

    public static Move CreateFullQualifiedMove(char fromPiece, string fromField, char toPiece, string toField, char promotion, bool isCheck, bool isMate, string shortAlgebraicNotation)
    {
        if (!IsValidMove(fromPiece, fromField, toPiece, toField, promotion))
        {
            return null;
        }

        Move newMove = new Move();
        newMove.fromPiece = fromPiece;
        newMove.fromField = StringToField(fromField);
        newMove.toPiece = toPiece;
        newMove.toField = StringToField(toField);
        newMove.promotion = promotion;
        newMove.check = isCheck;
        newMove.mate = isMate;
        newMove.shortAlgebraicNotation = shortAlgebraicNotation;

        return newMove;
    }
    #endregion

    private static bool IsValidMove(char fromPiece, string fromField, char toPiece, string toField, char promotion)
    {
        return IsAValidPiece(fromPiece) && IsAValidField(fromField) &&
            (IsAValidPiece(toPiece) || char.IsWhiteSpace(toPiece)) && IsAValidField(toField) && 
            IsAValidPromotion(promotion);
    }
    
    private static bool IsAValidPiece(char piece)
    {
        switch (piece)
        {
            case 'K':
            case 'Q':
            case 'R':
            case 'B':
            case 'N':
            case 'P':
                return true;
            default:
                return false;
        }
    }

    private static bool IsAValidField(string field)
    {
        if(field.Length != 2)
        {
            return false;
        }

        return IsValidRank(field[0]) && IsValidFile(field[1]);
    }

    private static bool IsValidRank(char rank)
    {
        switch (rank)
        {
            case 'a':
            case 'b':
            case 'c':
            case 'd':
            case 'e':
            case 'f':
            case 'g':
            case 'h':
                return true;
            default:
                return false;
        }
    }

    private static bool IsValidFile(char file)
    {
        switch (file)
        {
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
                return true;
            default:
                return false;
        }
    }

    private static bool IsAValidPromotion(char promotion)
    {
        switch (promotion)
        {
            case 'Q':
            case 'R':
            case 'B':
            case 'N':
            case ' ':
                return true;
            default:
                return false;
        }
    }

    private static Vector2Int StringToField(string fieldString) 
    {
        return new Vector2Int((char)fieldString[0] - 97, (char)fieldString[1] - 49);
    }

    public bool HasCapturedAPiece()
    {
        return !toPiece.Equals(' ');
    }

    public bool IsPawnMove()
    {
        return fromPiece.Equals('P');
    }

    public bool IsPromotion()
    {
        return !promotion.Equals(' ');
    }

    public bool IsCheck()
    {
        return check;
    }

    public bool IsMate()
    {
        return mate;
    }

    public char GetPromotion()
    {
        return promotion;
    }

    public Vector2Int GetFromField()
    {
        return fromField;
    }

    public Vector2Int GetToField()
    {
        return toField;
    }

    public char GetFromPiece()
    {
        return fromPiece;
    }

    public char GetToPiece()
    {
        return toPiece;
    }

    public bool IsShortCastle()
    {
        return GetSAN().Equals("O-O");
    }

    public bool IsLongCastle()
    {
        return GetSAN().Equals("O-O-O");
    }
    
    public bool IsCastle()
    {
        return IsShortCastle() || IsLongCastle();
    }

    override public string ToString()
    {
        return GetSAN();
    }

    public string GetSAN()
    {
        return shortAlgebraicNotation;
    }
}
