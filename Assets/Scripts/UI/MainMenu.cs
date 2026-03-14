using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Game";
    public GameObject settingsPanel;
    
    public void StartGame()
    {
        StartCoroutine(LoadGameWithDelay());
    }
    
    IEnumerator LoadGameWithDelay()
    {
        // Ждем немного чтобы звук успел проиграться
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }
    
    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
    
    public void QuitGame()
    {
        StartCoroutine(QuitWithDelay());
    }
    
    IEnumerator QuitWithDelay()
    {
        yield return new WaitForSeconds(0.3f);
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}