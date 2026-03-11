using UnityEngine;
using UnityEngine.EventSystems;

public class BoneSelectable : MonoBehaviour
{
    public BoneData boneData;

    private Renderer rend;
    private Color originalColor;


    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
    }

    public BoneSelectable Select(Color selectionColor)
    {
        rend.material.color = selectionColor;
        return this;
    }
    public void Deselect()
    {
        rend.material.color = originalColor;
    }

    //Подсветка нужной кости только в редакторе

#if UNITY_EDITOR
    void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelStart += HandleLevelStart;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnLevelStart -= HandleLevelStart;
    }

    private void HandleLevelStart(BoneData bone, int index)
    {
        if (boneData == bone)
            rend.material.color = Color.red;
        else rend.material.color = originalColor;
    }
#endif
}