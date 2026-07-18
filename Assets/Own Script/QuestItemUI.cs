using UnityEngine;
using TMPro;

public class QuestItemUI : MonoBehaviour
{
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questStatusText;
    public TextMeshProUGUI questProgressText;

    public void Setup(Quest quest)
    {
        if (questNameText != null) questNameText.text = quest.questName;

        if (questStatusText != null)
        {
            if (quest.isCompleted)
            {
                questStatusText.text = "/";
                questStatusText.color = Color.green;
            }
            else if (quest.currentProgress > 0)
            {
                questStatusText.text = $"{quest.currentProgress}/{quest.requiredCount}";
                questStatusText.color = new Color(1, 0.8f, 0);
            }
            else
            {
                questStatusText.text = "X";
                questStatusText.color = Color.gray;
            }
        }

        if (questProgressText != null)
        {
            int percent = Mathf.RoundToInt(quest.GetProgressPercent() * 100);
            questProgressText.text = $"{percent}%";
            questProgressText.color = quest.isCompleted ? Color.green : Color.white;
        }
    }
}