using UnityEngine;
using UnityEngine.UI;

public class SimpleMapButton : MonoBehaviour
{
    public GameObject mapContainer;  // 拖入 MapPanelContainer
    public GameObject introContainer; // 拖入 IntroPanelContainer
    private Button mapButton;
    public Button introButton;
    private bool isOpen = false;

    void Start()
    {
        introButton.onClick.AddListener(ToggleIntro);
        mapButton = GetComponent<Button>();
        mapButton.onClick.AddListener(ToggleMap);

        if (mapContainer != null)
            mapContainer.SetActive(false);
    }

    void ToggleMap()
    {
        isOpen = !isOpen;
        if (mapContainer != null)
            mapContainer.SetActive(isOpen);
    }

    void ToggleIntro()
    {
        if (introContainer != null)
            introContainer.SetActive(false);
    }
}