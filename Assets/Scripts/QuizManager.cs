using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Bone Database")]

    [SerializeField] private List<BoneData> bones;

    [Header("UI")]

    [SerializeField] private TMP_Text currentQuestionText;
    [SerializeField] private TMP_Text boneNamesText;
    [SerializeField] private TMP_Text correctAnswersCountText;
    [SerializeField] private Button confirmAnswerButton;
    [SerializeField] private StatusText answerStatusText;
    [SerializeField] private Button nextQuestionButton;
    [SerializeField] private Button finishButton;
    [SerializeField] private CanvasGroup resultsScreen;

    [Space]

    [SerializeField] private string currentQuestionTemplate = "Вопрос {0} из {1}";
    [SerializeField] private string boneNamesTemplate = "Выберите кости: {0}";
    [SerializeField] private string correctAnswersCountTemplate = "Правильных ответов: {0}";

    [Header("Quiz Settings")]

    [SerializeField] private int questionCount = 10;
    [SerializeField] private int questionGeneratorSeed = 0;
    [SerializeField] private bool highlightCorrectBonesForDebugging = false;

    private Dictionary<int, List<BoneData>> questions = new();
    private Dictionary<int, bool> answers = new();

    private int currentQuestion;

    private void Start()
    {
        if (bones == null || !bones.Any())
        {
            Debug.LogError("В Quiz Manager не заполнен массив костей!");
            return;
        }

        if (questionGeneratorSeed != 0)
            Random.InitState(questionGeneratorSeed);

        GenerateAndStartQuiz();
    }

    public void SetBones(IEnumerable<BoneData> bones)
    {
        this.bones = new List<BoneData>(bones);
    }

    public void GenerateAndStartQuiz()
    {
        GenerateQuiz();
        StartQuiz();
    }

    private void EnableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void DisableCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void GenerateQuiz()
    {
        questions.Clear();

        var distinctBoneNames = bones
            .Select(boneData => boneData.boneName)
            .Distinct()
            .ToList();

#if (UNITY_EDITOR)
        Debug.Log($"В базе найдено {distinctBoneNames.Count} уникальных названий костей");
#endif

        if (distinctBoneNames.Count < questionCount)
        {
#if (UNITY_EDITOR)
            Debug.LogError("Уникальных названий костей меньше, чем нужно сгенерировать вопросов!");
#endif
            return;
        }

        for (int i = 0; i < questionCount; i++)
        {
            int randomIndex = Random.Range(0, distinctBoneNames.Count);
            string randomBoneName = distinctBoneNames[randomIndex];

            var bonesWithTheSameName = bones
                .Where(bone => bone.boneName.Equals(randomBoneName))
                .ToList();

            questions[i + 1] = bonesWithTheSameName;

            distinctBoneNames.Remove(randomBoneName);
        }
    }

    public void StartQuiz()
    {
        answers.Clear();
        currentQuestion = 0;

        InitUI();
        ShowNextQuestion();
    }

    private void InitUI()
    {
        DisableCanvasGroup(resultsScreen);

        ShowCurrentQuestion(0);
        ShowBoneNames(Enumerable.Empty<string>());
        ShowCorrectAnswersCount(0);

        answerStatusText.SetMessage(string.Empty);

        nextQuestionButton.gameObject.SetActive(true);
        nextQuestionButton.interactable = false;

        finishButton.gameObject.SetActive(false);
    }

    public void ShowNextQuestion()
    {
        ResetBoneSelectionAndHighlight();

        currentQuestion++;

        if (!questions.TryGetValue(currentQuestion, out List<BoneData> bones))
        {
#if (UNITY_EDITOR)
            Debug.LogWarning("Попытка отобразить вопрос с несуществующим индексом.");
#endif
            return;
        }

        SetupNextQuestionAndFinishButtons();

        ShowBoneNames(bones.Select(boneData => boneData.boneName));
        ShowCurrentQuestion(currentQuestion);

        if (highlightCorrectBonesForDebugging)
            HighlightCorrectBones();

        confirmAnswerButton.interactable = true;
    }

    private void SetupNextQuestionAndFinishButtons()
    {
        if (currentQuestion < questionCount)
        {
            finishButton.gameObject.SetActive(false);

            nextQuestionButton.gameObject.SetActive(true);
            nextQuestionButton.interactable = false;
        }
        else
        {
            nextQuestionButton.gameObject.SetActive(false);

            finishButton.gameObject.SetActive(true);
            finishButton.interactable = false;
        }
    }

    private void HighlightCorrectBones()
    {
        var correctBoneIDs = questions[currentQuestion]
            .Select(boneData => boneData.boneID)
            .ToList();

        var correctSelectableBones = FindObjectsByType<BoneSelectable>(FindObjectsSortMode.None)
            .Where(bone => correctBoneIDs.Contains(bone.boneData.boneID));

        foreach (BoneSelectable bone in correctSelectableBones)
            bone.IsHighlightedForDebugging = true;
    }

    private void ResetBoneSelectionAndHighlight()
    {
        var selectableBones = FindObjectsByType<BoneSelectable>(FindObjectsSortMode.None);

        foreach (BoneSelectable bone in selectableBones)
            bone.ResetSelectionAndHighlight();
    }

    public void ConfirmAnswer()
    {
        confirmAnswerButton.interactable = false;

        var selectedBoneIDs = FindObjectsByType<BoneSelectable>(FindObjectsSortMode.None)
            .Where(bone => bone.IsSelected)
            .Select(bone => bone.boneData.boneID)
            .OrderBy(id => id)
            .ToList();

        var correctBoneIDs = questions[currentQuestion]
            .Select(boneData => boneData.boneID)
            .OrderBy(id => id)
            .ToList();

        bool correct = selectedBoneIDs.SequenceEqual(correctBoneIDs);

        answers[currentQuestion] = correct;

        if (correct)
            answerStatusText.SetSuccess("Ваш ответ верный!");
        else
            answerStatusText.SetFailure("Ваш ответ неверный!");

        ShowCorrectAnswersCount(answers.Values.Count(correct => correct));

        if (currentQuestion < questionCount)
            nextQuestionButton.interactable = true;
        else
            finishButton.interactable = true;
    }

    public void FinishQuiz()
    {
        EnableCanvasGroup(resultsScreen);
    }

    private void ShowCurrentQuestion(int question) => currentQuestionText.text = string.Format(currentQuestionTemplate, question > 0 ? question : 0, questionCount);
    private void ShowBoneNames(IEnumerable<string> names) => boneNamesText.text = string.Format(boneNamesTemplate, string.Join(", ", names.Distinct()));
    private void ShowCorrectAnswersCount(int count) => correctAnswersCountText.text = string.Format(correctAnswersCountTemplate, count);
}
