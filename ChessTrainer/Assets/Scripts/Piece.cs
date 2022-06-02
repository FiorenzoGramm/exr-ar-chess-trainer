using Microsoft.MixedReality.Toolkit.UI;
using System.Text;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string   type;
    public char     symbol;
    public bool     isWhite;

    public BoardController boardController;

    public bool isCurrentlyGrabbed;
    private Vector3 preGrabPosition;
    private Quaternion preGrabRotation;
    private Field lastTouchedField = null;

    private Piece()
    {
        type = "";
        symbol = '\0';
        isWhite = false;
        boardController = null;
        isCurrentlyGrabbed = false;
        preGrabPosition = Vector3.zero;
        preGrabRotation = Quaternion.identity;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isCurrentlyGrabbed)
        {
            Field touchedField = GetFieldFromCollider(other);
            if (touchedField != null && !touchedField.Equals(lastTouchedField))
            {
                if(lastTouchedField != null)
                {
                    lastTouchedField.DisableVisualizer();
                }
                lastTouchedField = touchedField;
                lastTouchedField.EnableVisualizer();
            }
        }
    }

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

    private Field GetFieldFromCollider(Collider collider)
    {
        if (collider.gameObject.CompareTag("Field"))
        {
            Field touchedField = collider.gameObject.GetComponent<Field>();
            if (touchedField == null)
            {
                throw new MissingComponentException($"Field {gameObject.name} is missing the {typeof(Field).ToString()} component.");
            }
            return touchedField;
        }
        return null;
    }

    public void DisableMoveable()
    {
        SetMoveable(false);
    }

    public void EnableMoveable()
    {
        SetMoveable(true);
    }

    public void SetMoveable(bool active)
    {
        GetComponent<ObjectManipulator>().enabled = active;
    }

    public static bool IsValidSymbol(char symbol)
    {
        return symbol.Equals('P') ||
            symbol.Equals('K') ||
            symbol.Equals('Q') ||
            symbol.Equals('B') ||
            symbol.Equals('N') ||
            symbol.Equals('R');
    }

    #region Method for listener
    public void OnGrab()
    {
        isCurrentlyGrabbed = true;
        preGrabPosition = transform.localPosition;
        preGrabRotation = transform.localRotation;
        boardController.DisablePieceMoveable();
    }

    public void OnRealease()
    {
        isCurrentlyGrabbed = false;
        transform.localPosition = preGrabPosition;
        transform.localRotation = preGrabRotation;
        boardController.EnablePieceMoveable();
        if (lastTouchedField != null)
        {
            lastTouchedField.DisableVisualizer();
            boardController.TryNewMovePiece(this, lastTouchedField);
        }
    }
    #endregion
}
