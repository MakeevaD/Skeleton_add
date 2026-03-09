using UnityEngine;

public class BoneSelectable : MonoBehaviour
{
    public BoneData boneData;

    private Renderer rend;
    private Color originalColor;

    private Color UnselectedColor => isHighlightedForDebugging ? Color.red : originalColor;

    private bool isHighlightedForDebugging = false;
    public bool IsHighlightedForDebugging
    {
        get => isHighlightedForDebugging;
        set
        {
            isHighlightedForDebugging = value;

            if (!isSelected && rend != null)
                rend.material.color = UnselectedColor;
        }
    }

    private bool isSelected = false;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;

            if (rend != null)
                rend.material.color = isSelected ? Color.yellow : UnselectedColor;
        }
    }

    void Awake()
    {
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        IsSelected = !IsSelected;

#if (UNITY_EDITOR)
        if (boneData != null)
        {
            Debug.Log($"Выбрана кость: {boneData.boneName} ID: {boneData.boneID}");
        }
        else
        {
            Debug.LogWarning($"Кость {gameObject.name} не содержит BoneData!");
            Debug.Log($"Выбрана кость: {gameObject.name}");
        }
#endif
    }

    public void ResetSelectionAndHighlight()
    {
        IsHighlightedForDebugging = false;
        IsSelected = false;
    }
}