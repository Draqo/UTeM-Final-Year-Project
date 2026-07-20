using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Quest
{
    public string questId;
    public string questName;
    public string questDescription;
    public List<string> requiredLocations;
    public int requiredCount;
    public int currentProgress;
    public bool isCompleted;

    public float GetProgressPercent()
    {
        if (requiredCount <= 0) return 0;
        return (float)currentProgress / requiredCount;
    }
}

public class AdventureQuestSystem : MonoBehaviour
{
    [Header("任务列表")]
    public List<Quest> quests = new List<Quest>();

    [Header("UI组件")]
    public TextMeshProUGUI currentQuestNameText;
    public TextMeshProUGUI currentQuestDescText;
    public Slider currentQuestSlider;
    public TextMeshProUGUI currentQuestProgressText;
    public Transform questListContainer;
    public GameObject questItemPrefab;

    private Dictionary<string, int> scannedLocations = new Dictionary<string, int>();

    public bool IsLocationScanned(string location)
    {
        return scannedLocations.ContainsKey(location);
    }

    void Start()
    {
        InitializeQuests();
        UpdateQuestUI();
        EventManager.OnLocationScanned += OnLocationScanned;
    }

    void InitializeQuests()
    {
        quests = new List<Quest>()
        {
            // Quest 1: 1 张
            new Quest
            {
                questId = "quest_1",
                questName = "Welcome to FTMK",
                questDescription = "Scan the FTMK main entrance to find the sign that reads \"FTMK\" and its full name.",
                requiredLocations = new List<string> { "ftmk_Teacher" },
                requiredCount = 1,
                currentProgress = 0,
                isCompleted = false
            },

            // Quest 2: 2 张
            new Quest
            {
                questId = "quest_2",
                questName = "2026-2030 Strategy Future of FTMK",
                questDescription = "Scan the plan & mission pictures.",
                requiredLocations = new List<string> { "plan_Teacher", "misi_Visi_Teacher" },
                requiredCount = 2,
                currentProgress = 0,
                isCompleted = false
            },
        
            // Quest 3: 2 张
            new Quest
            {
                questId = "quest_3",
                questName = "2 Prohibited Activities in College",
                questDescription = "Scan the no-smoking signs near the main entrance and the warning signs in the elevators.",
                requiredLocations = new List<string> { "elevator_Haro", "smoke_Haro" },
                requiredCount = 2,
                currentProgress = 0,
                isCompleted = false
            },

            // Quest 4: 1 张
            new Quest
            {
                questId = "quest_4",
                questName = "FTMK Floor Plan",
                questDescription = "Scan the floor plan.",
                requiredLocations = new List<string> { "m_FTMK_Teacher" },
                requiredCount = 1,
                currentProgress = 0,
                isCompleted = false
            },
        
            // Quest 5: 4 张
            new Quest
            {
                questId = "quest_5",
                questName = "4 Main Departments in the FTMK",
                questDescription = "Scan all 4 departments icons.",
                requiredLocations = new List<string> { "ai_Megamen", "bitm_Megamen", "security_Megamen", "faix_Megamen" },
                requiredCount = 4,
                currentProgress = 0,
                isCompleted = false
            },
        
            // Quest 6: 全部 10 张
            new Quest
            {
                questId = "quest_6",
                questName = "Ultimate Explorer",
                questDescription = "Scan all 10 locations! (0/10)",
                requiredLocations = new List<string> { "all" },
                requiredCount = 10,
                currentProgress = 0,
                isCompleted = false
            }
        };
    }

    void OnLocationScanned(string locationId)
    {
        Debug.Log($"📍 Scanned: {locationId}");

        if (scannedLocations.ContainsKey(locationId))
            scannedLocations[locationId]++;
        else
            scannedLocations[locationId] = 1;

        bool anyQuestUpdated = false;

        // 更新 "all" 任务
        foreach (var quest in quests)
        {
            if (quest.isCompleted) continue;
            if (quest.requiredLocations.Contains("all"))
            {
                quest.currentProgress = scannedLocations.Count;
                anyQuestUpdated = true;
                Debug.Log($"   ✅ 全局任务 '{quest.questName}' 进度: {quest.currentProgress}/{quest.requiredCount}");

                if (quest.currentProgress >= quest.requiredCount)
                {
                    CompleteQuest(quest);
                }
            }
        }

        // 更新普通任务
        for (int i = 0; i < quests.Count; i++)
        {
            Quest quest = quests[i];
            if (quest.isCompleted) continue;
            if (quest.requiredLocations.Contains("all")) continue;

            bool locationMatches = false;
            foreach (var loc in quest.requiredLocations)
            {
                if (loc == locationId)
                {
                    locationMatches = true;
                    break;
                }
            }

            if (locationMatches)
            {
                quest.currentProgress++;
                anyQuestUpdated = true;
                Debug.Log($"   ✅ 任务 '{quest.questName}' 进度: {quest.currentProgress}/{quest.requiredCount}");

                if (quest.currentProgress >= quest.requiredCount)
                {
                    CompleteQuest(quest);
                }
                break;
            }
        }

        if (anyQuestUpdated)
        {
            UpdateQuestUI();
        }
    }

    void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        UIManager.Instance?.ShowQuestComplete(quest.questName);
        UpdateQuestUI();
    }

    void UpdateQuestUI()
    {
        Quest currentQuest = GetCurrentQuest();
        if (currentQuest != null)
        {
            if (currentQuestNameText != null) currentQuestNameText.text = currentQuest.questName;
            if (currentQuestDescText != null) currentQuestDescText.text = currentQuest.questDescription;
            if (currentQuestSlider != null) currentQuestSlider.value = currentQuest.GetProgressPercent();
            if (currentQuestProgressText != null)
            {
                int percent = Mathf.RoundToInt(currentQuest.GetProgressPercent() * 100);
                currentQuestProgressText.text = $"{percent}%";
            }
        }
        else
        {
            if (currentQuestNameText != null) currentQuestNameText.text = "All Quests Completed!";
            if (currentQuestDescText != null) currentQuestDescText.text = "You are the FTMK Explorer Master!";
            if (currentQuestSlider != null) currentQuestSlider.value = 1;
            if (currentQuestProgressText != null) currentQuestProgressText.text = "100%";
        }
        UpdateQuestList();
    }

    void UpdateQuestList()
    {
        if (questListContainer == null || questItemPrefab == null) return;
        foreach (Transform child in questListContainer) Destroy(child.gameObject);
        foreach (Quest quest in quests)
        {
            GameObject item = Instantiate(questItemPrefab, questListContainer);
            QuestItemUI ui = item.GetComponent<QuestItemUI>();
            if (ui != null) ui.Setup(quest);
        }
    }

    // ⭐ 获取当前任务的目标位置（供雷达使用）
    public string GetCurrentTargetLocation()
    {
        Quest current = GetCurrentQuest();
        if (current == null) return null;

        foreach (var loc in current.requiredLocations)
        {
            if (!scannedLocations.ContainsKey(loc))
                return loc;
        }
        return null;
    }

    public Quest GetCurrentQuest()
    {
        foreach (Quest quest in quests) 
            if (!quest.isCompleted) return quest;
        return null;
    }

    void OnDestroy()
    {
        EventManager.OnLocationScanned -= OnLocationScanned;
    }
}