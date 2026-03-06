using UnityEngine;

public class BoneSelectable : MonoBehaviour
{
    public BoneData boneData;

    private Renderer rend;
    private Color originalColor;
    private bool isSelected = false;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend != null)
            originalColor = rend.material.color;
    }

    void OnMouseDown()
    {
        if (rend != null)
        {
            if (isSelected)
            {
                rend.material.color = originalColor;
                isSelected = false;
            }
            else
            {
                rend.material.color = Color.yellow;
                isSelected = true;
            }
        }

        if (boneData != null)
            Debug.Log("Выбрана кость: " + boneData.boneName + " ID: " + boneData.boneID);
        else
            Debug.Log("Выбрана кость: " + gameObject.name);
    }

    public void ResetColor()
    {
        if (rend != null)
        {
            rend.material.color = originalColor;
            isSelected = false;
        }
    }
}