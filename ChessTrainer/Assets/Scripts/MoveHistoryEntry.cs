using TMPro;
using UnityEngine;

public class MoveHistoryEntry : MonoBehaviour
{
    public TextMeshProUGUI m_NumberText;
    public TextMeshProUGUI m_WhitesMoveText;
    public TextMeshProUGUI m_BlacksMoveText;
 
    public void RemoveMove()
    {
        if(!m_BlacksMoveText.text.Equals(""))
        {
            m_BlacksMoveText.text = "";
        }
        else
        {   
            Destroy(gameObject);
        }
    }

    public void AddBlacksMove(string move)
    {
        m_BlacksMoveText.text = move;
    }

    public bool IsEmtpyEntry()
    {
        return m_NumberText.text.Equals("");
    }

    public void InitialiseEntry(int index, string move)
    {
        m_NumberText.text = (index / 2.0f + 1).ToString() + ".";
        m_WhitesMoveText.text = move;
        m_BlacksMoveText.text = "";
        gameObject.name = m_NumberText.text;
    }
}
