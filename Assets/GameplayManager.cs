using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public enum EGamePhase
    {
        None,
        DrawPhase,
        PlayPhase,
        CombatPhase,
        ScorePhase,
        EndPhase
    }

    public int initialHandCrads = 5;

    public static GameplayManager _instance;

    public UIHandController handController;
    public DeckController deckController;
    public BoardController boardController;

    public EGamePhase currentGamePhase;

    public Transform canvas;

    private void Awake()
    {
        _instance = this;
    }

    public static int GetInitialHandCards()
    {
        return _instance.initialHandCrads;
    }

    public static UIHandController GetHandController()
    {
        return _instance.handController;
    }

    public static DeckController GetDeckController()
    {
        return _instance.deckController;
    }

    public static BoardController GetBoardController()
    {
        return _instance.boardController;
    }

    public static Transform GetCanvas()
    {
        return _instance.canvas;
    }

}
