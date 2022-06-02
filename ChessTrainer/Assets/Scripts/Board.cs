using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject fieldsParent;

    public Field[,] fields = new Field[8, 8];

    public void InitialiseBoard()
    {
        Field[] allFields = GetComponentsInChildren<Field>();

        foreach (Field currentField in allFields)
        {
            fields[currentField.GetRank(), currentField.GetFile()] = currentField;
            currentField.piece = null;
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
        foreach(Transform currentChild in transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                currentChild.GetComponent<ObjectManipulator>().enabled = active;
            }
        }
    }

    public void DisableMoveableOnPieces()
    {
        SetMoveableOnPieces(false);
    }

    public void EnableMoveableOnPieces()
    {
        SetMoveableOnPieces(true);
    }

    public void SetMoveableOnPieces(bool active)
    {
        foreach(Field currentField in fields)
        {
            Piece currentPiece = currentField.piece;
            if (currentPiece != null && !currentPiece.isCurrentlyGrabbed)
            {
                currentField.piece.SetMoveable(active);
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
                Debug.Log(currentPiece);
                currentPiece.DisableMoveable();
            }
        }
    }

    public void OnRelease<T>(T eventData)
    {
        foreach (Field currentField in fields)
        {
            if (currentField.piece != null)
            {
                currentField.piece.EnableMoveable();
            }
        }
    }
    #endregion
}
