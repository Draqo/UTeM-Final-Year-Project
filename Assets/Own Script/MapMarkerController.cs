using UnityEngine;
using UnityEngine.UI;

public class MapMarkerController : MonoBehaviour
{
    [Header("Aras 1 地图的 Marker 标记")]
    public GameObject mar_ftmk;
    public GameObject mar_misi;
    public GameObject mar_plan;
    public GameObject mar_m_ftmk;
    public GameObject mar_elevator;
    public GameObject mar_smoke;
    public GameObject mar_faix;

    [Header("Aras 2 地图的 Marker 标记")]
    public GameObject mar_security;
    public GameObject mar_ai;
    public GameObject mar_bitm;

    private AdventureQuestSystem questSystem;

    void Start()
    {
        questSystem = FindFirstObjectByType<AdventureQuestSystem>();
        EventManager.OnLocationScanned += UpdateMarkers;
        EventManager.OnQuestCompleted += UpdateMarkers;
    }

    void UpdateMarkers(string _)
    {
        UpdateMarkers();
    }

    void UpdateMarkers()
    {
        // 先隐藏所有标记
        HideAllMarkers();

        if (questSystem == null) return;

        // ⭐ 获取当前任务对象
        Quest currentQuest = questSystem.GetCurrentQuest();
        if (currentQuest == null) return;

        // ⭐ 遍历当前任务的所有 requiredLocations
        foreach (string location in currentQuest.requiredLocations)
        {
            // 如果该位置已经扫描完成，跳过（不显示）
            if (questSystem.IsLocationScanned(location)) continue;

            // 显示该位置对应的标记
            ShowMarkerForLocation(location);
        }
    }

    void ShowMarkerForLocation(string location)
    {
        switch (location)
        {
            case "ftmk_Teacher":
                if (mar_ftmk != null) mar_ftmk.SetActive(true);
                break;
            case "plan_Teacher":
                if (mar_plan != null) mar_plan.SetActive(true);
                break;
            case "misi_Visi_Teacher":
                if (mar_misi != null) mar_misi.SetActive(true);
                break;
            case "m_ftmk_Teacher":
                if (mar_m_ftmk != null) mar_m_ftmk.SetActive(true);
                break;
            case "elevator_Haro":
                if (mar_elevator != null) mar_elevator.SetActive(true);
                break;
            case "smoke_Haro":
                if (mar_smoke != null) mar_smoke.SetActive(true);
                break;
            case "faix_Megamen":
                if (mar_faix != null) mar_faix.SetActive(true);
                break;
            case "security_Megamen":
                if (mar_security != null) mar_security.SetActive(true);
                break;
            case "ai_Megamen":
                if (mar_ai != null) mar_ai.SetActive(true);
                break;
            case "bitm_Megamen":
                if (mar_bitm != null) mar_bitm.SetActive(true);
                break;
            default:
                Debug.Log($"没有为 '{location}' 设置地图标记");
                break;
        }
    }

    void HideAllMarkers()
    {
        // Aras 1
        if (mar_ftmk != null) mar_ftmk.SetActive(false);
        if (mar_plan != null) mar_plan.SetActive(false);
        if (mar_misi != null) mar_misi.SetActive(false);
        if (mar_m_ftmk != null) mar_m_ftmk.SetActive(false);
        if (mar_elevator != null) mar_elevator.SetActive(false);
        if (mar_smoke != null) mar_smoke.SetActive(false);
        if (mar_faix != null) mar_faix.SetActive(false);

        // Aras 2
        if (mar_security != null) mar_security.SetActive(false);
        if (mar_ai != null) mar_ai.SetActive(false);
        if (mar_bitm != null) mar_bitm.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.OnLocationScanned -= UpdateMarkers;
        EventManager.OnQuestCompleted -= UpdateMarkers;
    }
}