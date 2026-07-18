using System.Collections;
using UnityEngine;

public class Interactive3DController : MonoBehaviour
{
    [Header("交互设置")]
    public string objectName = "Unknown Object";
    [TextArea(2, 4)] public string objectDescription = "Click for details";

    [Header("动画设置")]
    public AnimationClip interactAnimation;

    [Header("特效设置")]
    public GameObject hologramEffect;
    public AudioClip clickSound;

    private Animator animator;
    private AudioSource audioSource;
    private bool isInteracting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (GetComponent<Collider>() == null)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null) collider.size = renderer.bounds.size;
        }

        AddEventTriggerIfNeeded();
    }

    void AddEventTriggerIfNeeded()
    {
        UnityEngine.EventSystems.EventTrigger trigger = GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if (trigger == null) trigger = gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        bool hasPointerDown = false;
        foreach (var entry in trigger.triggers)
            if (entry.eventID == UnityEngine.EventSystems.EventTriggerType.PointerDown) { hasPointerDown = true; break; }

        if (!hasPointerDown)
        {
            var entry = new UnityEngine.EventSystems.EventTrigger.Entry();
            entry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { Interact(); });
            trigger.triggers.Add(entry);
        }
    }

    public void Interact()
    {
        if (isInteracting) return;
        StartCoroutine(InteractCoroutine());
    }

    IEnumerator InteractCoroutine()
    {
        isInteracting = true;
        if (animator != null && interactAnimation != null) animator.Play(interactAnimation.name);
        if (audioSource != null && clickSound != null) audioSource.PlayOneShot(clickSound);
        if (hologramEffect != null) { GameObject effect = Instantiate(hologramEffect, transform.position, Quaternion.identity); Destroy(effect, 2f); }
        UIManager.Instance?.ShowLocationInfo(objectName, objectDescription);
        yield return new WaitForSeconds(2f);
        isInteracting = false;
    }
}