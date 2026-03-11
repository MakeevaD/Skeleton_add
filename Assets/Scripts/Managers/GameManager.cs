using Assets.Scripts.Managers;
using System;
using UnityEngine;
[RequireComponent(typeof(QuizGenerator))]
[RequireComponent (typeof(SelectionManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field:SerializeField] public int levelsQuantity { get; private set; }
    public PointsManager pointsManager { get; private set; }
    private QuizGenerator quizGenerator;
    SelectionManager selectionManager;

    private BoneData[] quiz;
    private int currentIndex;

    public Action OnGameStart;
    public Action<BoneData, int> OnLevelStart;
    public Action OnGameEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        quizGenerator = GetComponent<QuizGenerator>();
        selectionManager = GetComponent<SelectionManager>();
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        quiz = quizGenerator.GenerateQuiz(levelsQuantity);
        pointsManager = new PointsManager();
        currentIndex = 0;

        OnGameStart?.Invoke();
        OnLevelStart?.Invoke(quiz[currentIndex], currentIndex);
    }

    public void NextLevel()
    {
        if (selectionManager.currentSelection != null)
        pointsManager.HandleAnswer(selectionManager.currentSelection.boneData == quiz[currentIndex]);
        currentIndex++;

        if (currentIndex >= quiz.Length)
        {
            EndGame();
            return;
        }

        OnLevelStart?.Invoke(quiz[currentIndex], currentIndex);
    }

    private void EndGame()
    {
        OnGameEnd?.Invoke();
    }
}
