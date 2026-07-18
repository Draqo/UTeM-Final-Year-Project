using UnityEngine;
using UnityEngine.UI;

public class SwitchMapPanel : MonoBehaviour
{
    [Header("📌 地图 Panel")]
    public GameObject panelAras1;
    public GameObject panelAras2;

    [Header("🔘 切换按钮")]
    public Button btnToAras1;
    public Button btnToAras2;

    void Start()
    {
        // 设置按钮文字
        if (btnToAras1 != null)
        {
            btnToAras1.onClick.AddListener(() => ShowMap(1));
        }

        if (btnToAras2 != null)
        {
            btnToAras2.onClick.AddListener(() => ShowMap(2));
        }

        ShowMap(1);
    }

    void ShowMap(int aras)
    {
        if (panelAras1 != null) panelAras1.SetActive(aras == 1);
        if (panelAras2 != null) panelAras2.SetActive(aras == 2);
    }
}