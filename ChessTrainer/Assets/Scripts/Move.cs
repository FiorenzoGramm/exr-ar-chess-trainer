using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public string   m_FromField;
    public string   m_ToField;
    public string   m_FromPiece;
    public string   m_ToPiece;      // replace with bool m_IsCapture -> GameManger looks for the piece
    public string   m_Promotion;
    public bool     m_IsCheck;
    public bool     m_IsMate;

    // Pawn move (no take, check, mate or promotion)
    public Move(string fromField, string toField)
    {
        this.m_FromField    = fromField;
        this.m_ToField      = toField;
        this.m_FromPiece    = "P";
        this.m_ToPiece      = "";
        this.m_Promotion    = "";
        this.m_IsCheck      = false;
        this.m_IsMate       = false;
    }
    
    // Any move (no take, check, mate or promotion)
    public Move(string fromPiece, string fromField, string toField)
    {
        this.m_FromField    = fromField;
        this.m_ToField      = toField;
        this.m_FromPiece    = fromPiece;
        this.m_ToPiece      = "";
        this.m_Promotion    = "";
        this.m_IsCheck      = false;
        this.m_IsMate       = false;
    }

    // Any move with take (no check, mate or poromotion)
    public Move(string fromPiece, string fromField, string toPiece, string toField)
    {
        this.m_FromField    = fromField;
        this.m_ToField      = toField;
        this.m_FromPiece    = fromPiece;
        this.m_ToPiece      = toPiece;
        this.m_Promotion    = "";
        this.m_IsCheck      = false;
        this.m_IsMate       = false;
    }

    // Any non promotion move
    public Move(string fromPiece, string fromField, string toPiece, string toField, bool check, bool mate)
    {
        this.m_FromField    = fromField;
        this.m_ToField      = toField;
        this.m_FromPiece    = fromPiece;
        this.m_ToPiece      = toPiece;
        this.m_Promotion    = "";
        this.m_IsCheck      = check;
        this.m_IsMate       = mate;
    }

    // Complete move
    public Move(string fromPiece, string fromField, string toPiece, string toField, string promotion, bool check, bool mate)
    {
        this.m_FromField    = fromField;
        this.m_ToField      = toField;
        this.m_FromPiece    = fromPiece;
        this.m_ToPiece      = toPiece;
        this.m_Promotion    = promotion;
        this.m_IsCheck      = check;
        this.m_IsMate       = mate;
    }

    // TODO: Create a constructor with just one string as parameter
    public static Move CreateMove(string move)
    {
        // Check if move starts with letter ('K','Q','R,'B','N'), if not, the from piece is a pawn ('P') otherwise the piece is the letter and skip -> fromPiece
        // If the next character is a ('a','b','c','d','e','f','g','h') save it and skip it
        // If ('1','2','3','4','5','6','7','8') concat to the one before and skip it
        // If we have 2 characters, we have the fromField -> fromField
        // If we have 1 character we have to search the rank or file for the piece on it -> fromField
        // If we have 0 characters, look all possible fields for the piece on it -> fromField
        // Find the next 2 characters which contain a field -> toField
        // If the string contains an 'x', take the piece from toField and save it under toPiece (skip 3 characters), otherwise toPiece is "" (skip 2 characters)-> toPiece
        // If next character is '=' set promotion to the next character and skip these two characters, otherwise to "" -> promotion
        // If next character is '+' set check to true
        // If next character is '#' set mate to true

        return null;
    }

    override public string ToString()
    {
        string result = "";

        if (!m_FromPiece.Equals("P"))
        {
            result += m_FromPiece;
        }
        if (!m_FromPiece.Equals("P"))
        {
            result += m_FromField;
        }
        if (m_ToPiece != "")
        {
            result += "x";
        }
        result += m_ToField;
        if(m_Promotion != "")
        {
            result += "=" + m_Promotion;
        }
        if (m_IsCheck)
        {
            result += "+";
        }
        if (m_IsMate)
        {
            result += "#";
        }
        return result;
    }
}
