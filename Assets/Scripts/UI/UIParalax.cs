using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public bool inverseDirection = false;
    
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position;
    }
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        
        float moveX = (mousePosition.x - Screen.width / 2) / (Screen.width / 2) * moveSpeed;
        float moveY = (mousePosition.y - Screen.height / 2) / (Screen.height / 2) * moveSpeed;
        
        if (inverseDirection)
        {
            moveX = -moveX;
            moveY = -moveY;
        }
        
        Vector3 targetPosition = new Vector3(
            startPosition.x + moveX,
            startPosition.y + moveY,
            startPosition.z
        );
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
    }
}
