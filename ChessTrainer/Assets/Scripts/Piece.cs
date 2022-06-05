using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Text;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string   type;
    public char     symbol;
    public bool     isWhite;

    public Animator modelAnimator;
    public BoardController boardController;

    public bool isCurrentlyGrabbed;
    private bool isInAnimation;
    private Vector3 preGrabPosition;
    private Quaternion preGrabRotation;
    private Field lastTouchedField = null;

    public void Initialise()
    {
        isCurrentlyGrabbed = false;
        isInAnimation = false;

        foreach (Transform currentChild in transform)
        {
            if (currentChild.CompareTag("Model"))
            {
                modelAnimator = currentChild.GetComponent<Animator>();
            }
        }
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

    public bool IsAnimated()
    {
        return modelAnimator != null;
    }

    public void GoToPosition(Vector3 position)
    {
        if (IsAnimated())
        {
            StartCoroutine(MovePieceTo(position));
        }
        else
        {
            transform.position = position;
        }
    }

    IEnumerator MovePieceTo(Vector3 targetPosition)
    {
        while (isInAnimation)
        {
            yield return null;
        }
        isInAnimation = true;

        float timeElapsed = 0;
        Vector3 startPosition = transform.position;
        float duration = 0.0f;

        Quaternion oldRotation = transform.rotation;
        float maxDegreesPerSecond = 180f;
        float angleToRotate = Vector3.SignedAngle(transform.forward, targetPosition - transform.position, Vector3.up);
        float currentAngle = angleToRotate;

        foreach (AnimationClip clip in modelAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Walk")
            {
                duration = clip.length;
            }
        }

        while (Mathf.Sign(currentAngle) * currentAngle > maxDegreesPerSecond * Time.deltaTime)
        {
            float tempAngle = maxDegreesPerSecond * Time.deltaTime * Mathf.Sign(currentAngle);
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), tempAngle);
            currentAngle -= tempAngle;
            yield return null;
        }
        currentAngle = -angleToRotate;
        transform.LookAt(targetPosition);

        modelAnimator.Play("Move");
        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        while (Mathf.Sign(currentAngle) * currentAngle > maxDegreesPerSecond * Time.deltaTime)
        {
            float tempAngle = maxDegreesPerSecond * Time.deltaTime * Mathf.Sign(currentAngle);
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), tempAngle);
            currentAngle -= tempAngle;
            yield return null;
        }
        transform.rotation = oldRotation;

        isInAnimation = false;
    }

    #region Method for listener
    public void OnGrab()
    {
        isCurrentlyGrabbed = true;
        preGrabPosition = transform.localPosition;
        preGrabRotation = transform.localRotation;
    }

    public void OnRealease()
    {
        isCurrentlyGrabbed = false;
        transform.localPosition = preGrabPosition;
        transform.localRotation = preGrabRotation;
        if (lastTouchedField != null)
        {
            lastTouchedField.DisableVisualizer();
            boardController.TryNewMovePiece(this, lastTouchedField);
        }
    }
    #endregion
}
