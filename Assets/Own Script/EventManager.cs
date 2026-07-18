using System;

public static class EventManager
{
    public static Action<string> OnLocationScanned;
    public static Action<string> OnDestinationReached;
    public static Action<string> OnQuestCompleted;

    public static void TriggerOnLocationScanned(string location)
    {
        OnLocationScanned?.Invoke(location);
        UnityEngine.Debug.Log($"[Event] 位置扫描: {location}");
    }

    public static void TriggerOnDestinationReached(string destination)
    {
        OnDestinationReached?.Invoke(destination);
        UnityEngine.Debug.Log($"[Event] 目标到达: {destination}");
    }

    public static void TriggerOnQuestCompleted(string questId)
    {
        OnQuestCompleted?.Invoke(questId);
        UnityEngine.Debug.Log($"[Event] 任务完成: {questId}");
    }
}