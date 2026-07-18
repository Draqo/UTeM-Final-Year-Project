using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    [Header("按钮")]
    public Button[] buttons;          // 10 个按钮

    [Header("Panel（纯图片）")]
    public GameObject[] panels;       // 10 个 Panel
    public Button[] closeButtons;     // 10 个关闭按钮

    void Start()
    {
        // 初始化所有 Panel 为隐藏状态
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // 为每个按钮绑定事件
        for (int i = 0; i < buttons.Length && i < panels.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OpenPanel(index));
        }

        // 为每个关闭按钮绑定事件
        for (int i = 0; i < closeButtons.Length && i < panels.Length; i++)
        {
            int index = i;
            closeButtons[i].onClick.AddListener(() => ClosePanel(index));
        }
    }

    void OpenPanel(int index)
    {
        // 关闭所有 Panel
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // 打开指定的 Panel
        if (panels[index] != null)
            panels[index].SetActive(true);
    }

    void ClosePanel(int index)
    {
        if (panels[index] != null)
            panels[index].SetActive(false);
    }
}