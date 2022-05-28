using System.Text;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string      type;
    public string      symbol;
    public bool        isWhite;

    override public string ToString()
    {
        StringBuilder pieceString = new StringBuilder();
        if (isWhite)
        {
            pieceString.Append("White ");
        }
        else
        {
            pieceString.Append("Black ");
        }

        pieceString.Append(type);
        
        return pieceString.ToString();
    }
}
