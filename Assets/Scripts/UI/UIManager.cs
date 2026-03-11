using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject game;
    [SerializeField] GameObject resultsPanel;

    private GameObject currentCanvas;

    private void Start()
    {
        currentCanvas = mainMenu;
        currentCanvas.SetActive(true);

        GameManager.Instance.OnGameStart += HandleGameStart;
        GameManager.Instance.OnGameEnd += HandleGameClose;
    }


    private void OnDestroy()
    {
            GameManager.Instance.OnGameStart -= HandleGameStart;
            GameManager.Instance.OnGameEnd -= HandleGameClose;
    }
    public void SwitchUI(GameObject canvasGroup)
    {
        currentCanvas.SetActive(false);
        currentCanvas = canvasGroup;
        currentCanvas.SetActive(true);
    }

    private void HandleGameStart()
    {
        SwitchUI(game);
    }

    private void HandleGameClose()
    {
        SwitchUI(resultsPanel);
    }
}
