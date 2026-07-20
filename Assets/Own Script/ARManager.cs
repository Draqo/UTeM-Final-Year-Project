using Unity.XR.CoreUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[System.Serializable]
public class MarkerData
{
    public string markerName;        // 必须和图片名称一致
    public string displayName;       // 显示名称
    [TextArea(2, 4)] public string description;  // 讲解文字
    public GameObject prefab;        // ⭐ 每个 Marker 独立模型
    public AudioClip audioClip;      // 讲解音频
}

public class ARManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Marker Data (每个Marker独立模型)")]
    public List<MarkerData> markerDataList = new List<MarkerData>();

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public float audioVolume = 0.8f;

    private GameObject currentActiveObject = null;
    private Dictionary<TrackableId, GameObject> spawnedObjects = new Dictionary<TrackableId, GameObject>();

    void Start()
    {
        // ⭐ 防止 markerDataList 为 null
        if (markerDataList == null)
        {
            markerDataList = new List<MarkerData>();
            Debug.LogWarning("⚠️ markerDataList 为 null，已自动初始化！");
        }

        // 检查是否有数据
        if (markerDataList.Count == 0)
        {
            Debug.LogWarning("⚠️ markerDataList 没有任何数据，请在 Inspector 中添加！");
        }
        else
        {
            Debug.Log($"✅ markerDataList 有 {markerDataList.Count} 条数据");
        }

        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        ValidateMarkerData();
    }

    void ValidateMarkerData()
    {
        if (markerDataList == null || markerDataList.Count == 0)
        {
            Debug.LogWarning("⚠️ 没有 Marker 数据可验证！");
            return;
        }

        Debug.Log($"📋 共加载 {markerDataList.Count} 个Marker数据");
        foreach (var data in markerDataList)
        {
            if (data != null)
            {
                Debug.Log($"   - {data.markerName} | 模型: {(data.prefab != null ? "✅" : "❌")} | 音频: {(data.audioClip != null ? "✅" : "❌")}");
            }
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added) OnImageDetected(trackedImage);
        foreach (var trackedImage in args.updated) OnImageUpdated(trackedImage);
        foreach (var trackedImage in args.removed) OnImageRemoved(trackedImage);
    }

    void OnImageDetected(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        TrackableId id = trackedImage.trackableId;

        MarkerData markerData = GetMarkerData(imageName);
        if (markerData == null)
        {
            Debug.LogWarning($"⚠️ 未找到Marker数据: {imageName}");
            return;
        }

        if (markerData.prefab == null)
        {
            Debug.LogWarning($"⚠️ Marker '{imageName}' 没有指定模型");
            return;
        }

        DestroyCurrentActiveObject();

        GameObject spawned = Instantiate(markerData.prefab, trackedImage.transform.position, trackedImage.transform.rotation);
        spawned.transform.SetParent(trackedImage.transform);
        spawned.transform.localPosition = Vector3.zero;

        // ⭐ 添加 ARAnchor 组件
        ARAnchor anchor = spawned.GetComponent<ARAnchor>();
        if (anchor == null)
        {
            anchor = spawned.AddComponent<ARAnchor>();
        }

        switch (imageName)
        {
            case "ftmk_Teacher":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localRotation = new Quaternion(0, -135, 0, 1);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            case "plan_Teacher":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localRotation = new Quaternion(0, -115, 0, 1);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            case "misi_Visi_Teacher":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localRotation = new Quaternion(0, 180, 0, 1);
                spawned.transform.localPosition = new Vector3(0.05f, 0f, 0.24f);
                break;
            case "m_FTMK_Teacher":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localRotation = new Quaternion(0, 180, 0, 1);
                spawned.transform.localPosition = new Vector3(0.05f, 0f, 0.24f);
                break;
            case "elevator_Haro":
                spawned.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                spawned.transform.localRotation = Quaternion.Euler(-90, 180, 0);
                spawned.transform.localPosition = new Vector3(0.15f, 0.1f, 0.24f);
                break;
            case "smoke_Haro":
                spawned.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
                spawned.transform.localRotation = Quaternion.Euler(-90, 180, 0);
                spawned.transform.localPosition = new Vector3(0.15f, 0.1f, 0.24f);
                break;
            case "faix_Megamen":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            case "bitm_Megamen":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            case "security_Megamen":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            case "ai_Megamen":
                spawned.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                spawned.transform.localPosition = new Vector3(0f, 0f, 0.24f);
                break;
            default:
                spawned.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                spawned.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
        }

        // ⭐ 修复材质设置（使用 URP 着色器）
        Renderer renderer = spawned.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            // 如果模型没有材质，创建一个 URP Lit 材质
            if (renderer.material == null || renderer.material.shader == null)
            {
                Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
                if (urpLit != null)
                {
                    Material mat = new Material(urpLit);
                    mat.color = Color.white;
                    renderer.material = mat;
                    Debug.Log($"✅ 材质已设置为 URP Lit");
                }
            }
        }

        currentActiveObject = spawned;
        spawnedObjects.Add(id, spawned);

        PlayAudio(markerData.audioClip);

        UIManager.Instance?.UpdateFeedbackText($"{markerData.displayName}");
        EventManager.TriggerOnLocationScanned(imageName);

        Debug.Log($"✅ 检测到: {markerData.displayName}");
    }

    void OnImageUpdated(ARTrackedImage trackedImage)
    {
        TrackableId id = trackedImage.trackableId;
        if (spawnedObjects.ContainsKey(id))
        {
            //spawnedObjects[id].transform.position = trackedImage.transform.localPosition + new Vector3(0f, 0.15f, 0.2f);
        }
    }

    void OnImageRemoved(ARTrackedImage trackedImage)
    {
        TrackableId id = trackedImage.trackableId;
        if (spawnedObjects.ContainsKey(id))
        {
            Destroy(spawnedObjects[id]);
            spawnedObjects.Remove(id);
            if (currentActiveObject != null && currentActiveObject == spawnedObjects[id])
                currentActiveObject = null;
        }
    }

    MarkerData GetMarkerData(string markerName)
    {
        // ⭐ 安全检查：如果列表为空，直接返回 null
        if (markerDataList == null || markerDataList.Count == 0)
        {
            Debug.LogError($"❌ markerDataList 为空！无法查找 '{markerName}'。请在 Inspector 中添加数据！");
            return null;
        }

        // 遍历查找
        foreach (var data in markerDataList)
        {
            if (data != null && data.markerName.ToLower() == markerName.ToLower())
                return data;
        }

        Debug.LogWarning($"⚠️ 未找到 Marker 数据: {markerName}");
        return null;
    }

    void PlayAudio(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.volume = audioVolume;
        audioSource.Play();
        Debug.Log($"🔊 播放音频: {clip.name}");
    }

    void DestroyCurrentActiveObject()
    {
        if (currentActiveObject != null)
        {
            foreach (var pair in spawnedObjects)
            {
                if (pair.Value == currentActiveObject)
                {
                    spawnedObjects.Remove(pair.Key);
                    break;
                }
            }
            Destroy(currentActiveObject);
            currentActiveObject = null;
            Debug.Log("🗑️ 已删除之前的3D物体");
        }
    }

    void OnDestroy()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
}