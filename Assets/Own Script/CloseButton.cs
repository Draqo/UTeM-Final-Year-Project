using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public GameObject targetPanel;  // 要关闭的 Panel

    void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                if (targetPanel != null)
                    targetPanel.SetActive(false);
            });
        }
    }
}