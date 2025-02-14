using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModuleUI : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI rivalScore;

    public TextMeshProUGUI playerCombo;
    public TextMeshProUGUI rivalCombo;

    public Canvas UICanvas;
    public RectTransform UICanvasRectTransform;

    public static ModuleUI _instance;

    public GameObject GameFinishedMenu;
    public GameObject CardsFolder;

    public TextMeshProUGUI victoryText;
    public string TextForWin = "Victory!";
    public string TextForLose = "Defeat...";
    public string TextForDraw = "Draw";

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        UpdateScores();
    }

    private void OnEnable()
    {
        GameplayManager.onGameFinished += OnGameFinished;
    }

    private void OnDisable()
    {
        GameplayManager.onGameFinished -= OnGameFinished;
    }

    public static Canvas GetCanvas()
    {
        return _instance.UICanvas;
    }

    public static RectTransform GetCanvasRectTransform()
    {
        return _instance.UICanvasRectTransform;
    }

    public static void UpdateScores()
    {
        _instance.playerScore.text = "Score: " + GameplayManager.GetPlayerController().currentScore.ToString();
        _instance.rivalScore.text = "Score: " + GameplayManager.GetRivalController().currentScore.ToString();

        _instance.playerCombo.text = "Combo: " + GameplayManager.GetPlayerController().currentCombo.ToString();
        _instance.rivalCombo.text = "Combo: " + GameplayManager.GetRivalController().currentCombo.ToString();
    }

    public static void SetGameFinishedMenuStatus(bool status)
    {
        _instance.GameFinishedMenu.SetActive(status);
    }

    private void OnGameFinished(GameplayManager.EGameResult gameResult)
    {
        SetGameFinishedMenuStatus(true);
        SetGameResultText(gameResult);
    }

    private void SetGameResultText(GameplayManager.EGameResult gameResult)
    {
        switch (gameResult)
        {
            case GameplayManager.EGameResult.Victory:
                victoryText.text = TextForWin;
                break;
            case GameplayManager.EGameResult.Defeat:
                victoryText.text = TextForLose;
                break;
            case GameplayManager.EGameResult.Draw:
                victoryText.text = TextForDraw;
                break;
        }
    }
}
