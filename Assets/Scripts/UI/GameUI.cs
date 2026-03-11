using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [Header("MainGame panel")]
    [SerializeField] private TextMeshProUGUI currentQuestionText;
    [SerializeField] private TextMeshProUGUI boneNameText;
    [SerializeField] private TextMeshProUGUI correctAnswersCountText;
    [Header("Results panel")]
    [SerializeField] private TextMeshProUGUI results;

    private void Start()
    {
        GameManager.Instance.OnLevelStart += HandleLevelStart;
        GameManager.Instance.OnGameEnd += HandleGameEnd;
    }
    private void HandleLevelStart(BoneData correctBone, int currentLevel)
    {
        currentQuestionText.text = $"Вопрос {currentLevel + 1} из {GameManager.Instance.levelsQuantity}";

        boneNameText.text = $"Выберите кость: {correctBone.boneName}";

        correctAnswersCountText.text = $"Очки: {GameManager.Instance.pointsManager.points} / {GameManager.Instance.levelsQuantity}";
    }
    private void HandleGameEnd()
    {
        results.text = $"Очки: {GameManager.Instance.pointsManager.points} / {GameManager.Instance.levelsQuantity}";
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnLevelStart -= HandleLevelStart;
    }
}
