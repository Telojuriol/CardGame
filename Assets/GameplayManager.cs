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

    public PlayerController playerController;
    public RivalController rivalController;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        StartCoroutine(TurnLoop());
    }

    IEnumerator TurnLoop()
    {
        while (true)
        {
            yield return new WaitUntil(() => AllCardsPlayed());
            currentGamePhase = EGamePhase.CombatPhase;
            Debug.Log("All cards played!");
            yield return new WaitForSeconds(1f);
            DetermineRoundWinner();
            yield return new WaitForSeconds(2f);
            RemoveCardsFromBoard();
        }    
    }

    private bool AllCardsPlayed()
    {
        return boardController.AllCardsPlayed();
    }

    private void DetermineRoundWinner()
    {
        CardController playerCard = GetPlayedCard(true);
        CardController rivalCard = GetPlayedCard(false);
        if (playerCard.cardNumber > rivalCard.cardNumber)
        {
            playerController.RoundWin();
            rivalController.RoundLost();
        }
        else if (playerCard.cardNumber == rivalCard.cardNumber)
        {
            playerController.RoundTied();
            rivalController.RoundTied();
        }
        else
        {
            playerController.RoundLost();
            rivalController.RoundWin();
        }
    }

    private void RemoveCardsFromBoard()
    {
        boardController.ClearBoard();
    }

    private CardController GetPlayedCard(bool playerCard)
    {
        CombatantController.ECombatantType combatatntToGet = playerCard ? CombatantController.ECombatantType.Player : CombatantController.ECombatantType.Rival;
        if (boardController.playableSockets[0].playedCard.combatantOwner.combatantType == combatatntToGet)
        {
            return boardController.playableSockets[0].playedCard;
        }
        else
        {
            return boardController.playableSockets[1].playedCard;
        }
    }

    private bool NoCardsOnHands()
    {
        return false;
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

    public static PlayerController GetPlayerController()
    {
        return _instance.playerController;
    }

    public static RivalController GetRivalController()
    {
        return _instance.rivalController;
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
