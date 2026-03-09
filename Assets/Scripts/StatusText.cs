using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StatusText : MonoBehaviour
{
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failureColor = Color.red;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetMessage(string message) => SetStatus(normalColor, message);
    public void SetSuccess(string message) => SetStatus(successColor, message);
    public void SetFailure(string message) => SetStatus(failureColor, message);

    private void SetStatus(Color color, string message)
    {
        text.color = color;
        text.text = message;
    }
}
