using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.Events;

public class Board : MonoBehaviour
{
    public GameObject fieldsParent;

    public Field[,] fields = new Field[8, 8];

    private BoardController boardController;

    public void InitialiseBoard(BoardController controller)
    {
        boardController = controller;

        Field[] allFields = GetComponentsInChildren<Field>();

        foreach (Field currentField in allFields)
        {
            fields[currentField.GetRank(), currentField.GetFile()] = currentField;
            currentField.piece = null;
            currentField.DisableColldier();
        }

        UpdateBoxCollider();
    }

    public void UpdateBoxCollider()
    {
        BoxCollider boardCollider = GetComponent<BoxCollider>();
        BoxCollider modelCollider = GetModelBoxCollider();

        if (modelCollider == null)
        {
            throw new MissingComponentException("The model of board needs BoxCollider.");
        }

        boardCollider.size = modelCollider.size;
        boardCollider.center = modelCollider.center;
        boardCollider.isTrigger = modelCollider.isTrigger;
    }

    private BoxCollider GetModelBoxCollider()
    {
        foreach (Transform currentChild in transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                return currentChild.GetComponent<BoxCollider>();
            }
        }
        return null;
    }

    public void DestroyPieceOnPosition(Vector2Int position)
    {
        if (!Field.IsValidPosition(position))
        {
            return;
        }

        Piece piece = fields[position.x, position.y].piece;
        if (piece != null)
        {
            Destroy(piece.gameObject);
            fields[position.x, position.y].piece = null;
        }
    }

    public bool IsEmptyFieldByPosition(Vector2Int position)
    {
        if (!Field.IsValidPosition(position))
        {
            throw new InvalidFieldException($"Cannot check a field due invalid position ({position})", position);
        }

        return fields[position.x, position.y].piece == null;
    }

    public void DisableGrabbableOfNonGrabbedPieces()
    {
        foreach (Field currentField in fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null)
            {
                currentPiece.DisableGrabbableWhenNotGrabbed();
            }
        }
    }

    public void EnableGrabbable()
    {
        GetObjectManipulator().enabled      = true;
        GetModelBoxCollider().enabled       = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void DisableGrabbable()
    {
        GetObjectManipulator().enabled      = false;
        GetModelBoxCollider().enabled       = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void DisableFieldCollider()
    {
        foreach(Field currentField in fields)
        {
            currentField.DisableColldier();
        }
    }

    public void EnableFieldCollider()
    {
        foreach (Field currentField in fields)
        {
            currentField.EnableCollider();
        }
    }

    private ObjectManipulator GetObjectManipulator()
    {
        ObjectManipulator objManipulator = null;

        foreach(Transform currentChild in transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                objManipulator = currentChild.GetComponent<ObjectManipulator>();
            }
        }

        if (objManipulator == null)
        {
            throw new MissingComponentException($"Board {gameObject.name} is missing the {typeof(ObjectManipulator)} component.");
        }

        return objManipulator;
    }

    public void EnableGrabbableOfPieces()
    {
        foreach (Field currentField in fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null)
            {
                currentPiece.EnableGrabbable();
            }
        }
    }

    #region Method for listener
    public void OnGrab<T>(T eventData)
    {
        foreach(Field currentField in fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null)
            {
                currentPiece.DisableGrabbable();
            }
        }
        
    }

    public void OnRelease<T>(T eventData)
    {
        foreach (Field currentField in fields)
        {
            if (currentField.piece != null)
            {
                currentField.piece.EnableGrabbable();
            }
        }
    }
    #endregion
}
