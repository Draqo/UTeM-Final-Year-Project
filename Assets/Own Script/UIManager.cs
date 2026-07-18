using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject questPanel;
    public GameObject infoPanel;
    public GameObject notificationPanel;

    [Header("Texts")]
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI locationInfoText;
    public TextMeshProUGUI notificationText;

    [Header("Settings")]
    public float infoPanelDuration = 5f;
    public float notificationDuration = 3f;

    private Coroutine infoCoroutine;
    private Coroutine notificationCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetQuestPanel(true);
        SetInfoPanel(false);
        SetNotificationPanel(false);
        if (feedbackText != null) feedbackText.text = "AR Ready! Scan FTMK markers";
    }

    public void SetQuestPanel(bool isActive) { if (questPanel != null) questPanel.SetActive(isActive); }
    public void SetInfoPanel(bool isActive) { if (infoPanel != null) infoPanel.SetActive(isActive); }
    public void SetNotificationPanel(bool isActive) { if (notificationPanel != null) notificationPanel.SetActive(isActive); }

    public void UpdateFeedbackText(string message) { if (feedbackText != null) feedbackText.text = message; }

    public void ShowLocationInfo(string title, string description)
    {
        if (locationInfoText != null) locationInfoText.text = $"{title}\n{description}";
        if (infoCoroutine != null) StopCoroutine(infoCoroutine);
        infoCoroutine = StartCoroutine(AutoHidePanel(infoPanel, infoPanelDuration));
    }

    public void ShowNotification(string message)
    {
        if (notificationText != null) notificationText.text = message;
        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);
        notificationCoroutine = StartCoroutine(AutoHidePanel(notificationPanel, notificationDuration));
    }

    public void ShowQuestComplete(string questName) { ShowNotification($"Quest Complete: {questName}"); }

    IEnumerator AutoHidePanel(GameObject panel, float delay)
    {
        if (panel == null) yield break;
        panel.SetActive(true);
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
    }
}