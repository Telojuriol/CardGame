using System;
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

    public HandController handController;
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

    public static HandController GetHandController()
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

    [Serializable]
    public class PlayableSocket
    {
        public Transform anchor;
        [HideInInspector] public CardController playedCard;

        public PlayableSocket(Transform anchor)
        {
            this.anchor = anchor;
        }

        public bool IsSocketFree()
        {
            return playedCard == null;
        }
    }

}
