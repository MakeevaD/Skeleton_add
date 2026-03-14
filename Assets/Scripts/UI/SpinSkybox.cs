using UnityEngine;

public class SpinSkybox : MonoBehaviour
{
    public float rotationSpeed = 1f;
    
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
