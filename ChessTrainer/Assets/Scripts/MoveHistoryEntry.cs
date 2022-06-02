using TMPro;
using UnityEngine;

public class MoveHistoryEntry : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public TextMeshProUGUI whitesMoveText;
    public TextMeshProUGUI blacksMoveText;
 
    public void RemoveMove()
    {
        if(!blacksMoveText.text.Equals(""))
        {
            blacksMoveText.text = "";
        }
        else
        {   
            Destroy(gameObject);
        }
    }

    public void AddBlacksMove(string move)
    {
        blacksMoveText.text = move;
    }

    public bool IsEmtpyEntry()
    {
        return numberText.text.Equals("");
    }

    public void InitialiseEntry(int index, string move)
    {
        numberText.text = (index / 2.0f + 1).ToString() + ".";
        whitesMoveText.text = move;
        blacksMoveText.text = "";
        gameObject.name = numberText.text;
    }
}
