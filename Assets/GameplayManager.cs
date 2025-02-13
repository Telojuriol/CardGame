using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameplayManager;

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

    public enum EEnemyType
    {
        AI,
        Player
    }

    public int initialHandCrads = 5;

    public delegate void OnGameFinished(bool localPlayerWin);
    public delegate void OnStartedWaitingForCards();

    public static event OnStartedWaitingForCards waitingForCards;
    public static event OnGameFinished onGameFinished;

    public static GameplayManager _instance;

    public HandController handController;
    public DeckController deckController;
    public BoardController boardController;

    public EGamePhase currentGamePhase;

    public PlayerController playerController;
    public RivalController rivalController;

    public EEnemyType enemyType = EEnemyType.AI;

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
        bool GameFinished = false;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            //waitingForCards?.Invoke();
            yield return new WaitUntil(() => AllCardsPlayed());
            currentGamePhase = EGamePhase.CombatPhase;
            yield return new WaitForSeconds(0.25f);
            TurnUpCards();
            yield return new WaitUntil(() => AllCardsLookingUp());
            yield return new WaitForSeconds(0.5f);
            DetermineRoundWinner();
            yield return new WaitForSeconds(2f);
            if(NoCardsOnHands()) break;
            RemoveCardsFromBoard();
        }
        onGameFinished?.Invoke(true);
    }

    private bool AllCardsPlayed()
    {
        return boardController.AllCardsPlayed();
    }

    private void TurnUpCards()
    {
        for(int i = 0; i < boardController.playableSockets.Count; i++)
        {
            if(boardController.playableSockets[i].playedCard.isFaceDown) boardController.playableSockets[i].playedCard.TurnCard();
        }       
    }

    private bool AllCardsLookingUp()
    {
        bool allFacingUp = true;
        for (int i = 0; i < boardController.playableSockets.Count; i++)
        {
            allFacingUp = allFacingUp || !boardController.playableSockets[i].playedCard.isFaceDown;
        }
        return allFacingUp;
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
        return rivalController.ownHand.currentNumberOfCardsOnHand == 0 && playerController.ownHand.currentNumberOfCardsOnHand == 0;
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

    public void OnGoToMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

}

[Serializable]
public class PlayableSocket
{
    public Transform anchor;
    public CardController playedCard;
    public RectTransform anchorRectTransform;

    public PlayableSocket(Transform anchor, RectTransform anchorRectTransform)
    {
        this.anchor = anchor;
        this.anchorRectTransform = anchorRectTransform;
    }

    public bool IsSocketFree()
    {
        return playedCard == null;
    }
}
