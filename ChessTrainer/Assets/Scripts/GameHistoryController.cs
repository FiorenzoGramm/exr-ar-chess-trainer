using UnityEngine;
using UnityEngine.UI;

public class GameHistoryController : MonoBehaviour
{
    public MoveHistoryEntry moveHistoryEntryPrefab;
    public VerticalLayoutGroup contentElement;
    public Scrollbar verticalScrollbar;

    public void Reset()
    {
        MoveHistoryEntry[] entries = contentElement.GetComponentsInChildren<MoveHistoryEntry>();

        foreach (MoveHistoryEntry entry in entries)
        {
            Destroy(entry.gameObject);
        }
    }

    public void AddMove(int index, string move)
    {
        if(IsWhite(index))
        {
            MoveHistoryEntry newEntry = Instantiate(moveHistoryEntryPrefab, contentElement.transform);
            newEntry.InitialiseEntry(index, move);
        }
        else
        {
            MoveHistoryEntry[] entries = contentElement.GetComponentsInChildren<MoveHistoryEntry>();
            entries[entries.Length - 1].AddBlacksMove(move);
        }
        verticalScrollbar.value = 0.0f;
    }

    public void RemoveMove()
    {
        MoveHistoryEntry[] entries = contentElement.GetComponentsInChildren<MoveHistoryEntry>();
        MoveHistoryEntry lastEntry = entries[entries.Length - 1];

        lastEntry.RemoveMove();

        if (lastEntry.IsEmtpyEntry())
        {
            Destroy(lastEntry);
        }
    }

    private bool IsWhite(int index)
    {
        return index % 2 == 0;
    }
}
