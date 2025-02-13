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

    public static ModuleUI _instance;

    public GameObject GameFinishedMenu;

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

    private void OnGameFinished(bool localPlayerWon)
    {
        SetGameFinishedMenuStatus(true);
    }
}
