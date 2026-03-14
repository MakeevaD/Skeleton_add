using UnityEngine;

public class GameEventsSound : MonoBehaviour
{
    private SelectionManager selectionManager;
    private bool wasBoneSelected = false;
    
    void Start()
    {
        selectionManager = FindObjectOfType<SelectionManager>();
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart += PlayBackgroundMusic;
            GameManager.Instance.OnLevelStart += OnLevelStart;
            GameManager.Instance.OnGameEnd += OnGameEnd;
        }
    }
    
    void Update()
    {
        if (selectionManager != null && selectionManager.currentSelection != null && !wasBoneSelected)
        {
            if (AudioManager.instance != null)
                AudioManager.instance.Play("BoneSelect");
            wasBoneSelected = true;
        }
        
        if (selectionManager != null && selectionManager.currentSelection == null)
        {
            wasBoneSelected = false;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            CheckAnswer();
        }
    }
    
    void CheckAnswer()
    {
        if (selectionManager == null || selectionManager.currentSelection == null) return;
        if (GameManager.Instance == null) return;
        
        BoneData selectedBone = selectionManager.currentSelection.boneData;
        
        // Получаем текущую кость через рефлексию
        System.Reflection.FieldInfo quizField = typeof(GameManager).GetField("quiz", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        System.Reflection.FieldInfo indexField = typeof(GameManager).GetField("currentIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (quizField != null && indexField != null)
        {
            var quiz = quizField.GetValue(GameManager.Instance) as System.Array;
            int currentIndex = (int)indexField.GetValue(GameManager.Instance);
            
            if (quiz != null && currentIndex < quiz.Length)
            {
                BoneData currentBone = quiz.GetValue(currentIndex) as BoneData;
                
                if (AudioManager.instance != null)
                {
                    if (selectedBone == currentBone)
                        AudioManager.instance.Play("CorrectAnswer");
                    else
                        AudioManager.instance.Play("WrongAnswer");
                }
            }
        }
    }
    
    void PlayBackgroundMusic()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.Play("BackgroundMusic");
    }
    
    void OnLevelStart(BoneData bone, int index)
    {
        wasBoneSelected = false;
    }
    
    void OnGameEnd()
    {
        // Проигрываем звук успеха при завершении игры
        if (AudioManager.instance != null)
            AudioManager.instance.Play("CorrectAnswer");
    }
    
    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameStart -= PlayBackgroundMusic;
            GameManager.Instance.OnLevelStart -= OnLevelStart;
            GameManager.Instance.OnGameEnd -= OnGameEnd;
        }
    }
}
