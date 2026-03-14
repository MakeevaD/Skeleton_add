using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(PlaySound);
        }
    }
    
    void OnEnable()
    {
        if (button == null)
            button = GetComponent<Button>();
            
        if (button != null)
        {
            // Переподписываемся при каждом включении
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(PlaySound);
        }
    }
    
    void PlaySound()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("ButtonClick");
    }
}