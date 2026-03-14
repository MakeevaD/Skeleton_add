using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public string musicName = "BackgroundMusic";
    
    private static BackgroundMusic instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    void Start()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play(musicName);
            
        SceneManager.sceneLoaded += (scene, mode) => 
        {
            if (AudioManager.instance != null)
                AudioManager.instance.Play(musicName);
        };
    }
}
