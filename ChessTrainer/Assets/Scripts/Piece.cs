using UnityEngine;

public class Piece : MonoBehaviour
{
    public string      m_Type;
    public string      m_Symbol;
    public bool        m_IsWhite;

    override public string ToString()
    {
        string res = "";
        if (m_IsWhite)
        {
            res += "White ";
        }
        else
        {
            res += "Black ";
        }
        res += m_Type;
        return res;
    }
}
