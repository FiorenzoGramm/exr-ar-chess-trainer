using UnityEngine;

public class Field : MonoBehaviour
{
    public Vector2Int position;
    public Piece piece;
    public GameObject visualizer;

    public Vector2Int GetPosition()
    {
        return new Vector2Int(position.x, position.y);
    }

    public int GetRank()
    {
        return position.x;
    }

    public int GetFile()
    {
        return position.y;
    }

    public static bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x <= 7 && position.y >= 0 && position.y <= 7;
    }

    public override string ToString()
    {
        string res = "";
        res += (char)(65 + position.x);
        res += (char)(48 + position.y + 1);
        return res;
    }

    public void EnableVisualizer()
    {
        SetVisualizer(true);
    }

    public void DisableVisualizer()
    {
        SetVisualizer(false);
    }

    public void SetVisualizer(bool active)
    {
        if(visualizer != null)
        {
            visualizer.SetActive(active);
        }
    }

    public void EnableCollider()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void DisableColldier()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }
}
