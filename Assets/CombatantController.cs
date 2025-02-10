using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CombatantController : MonoBehaviour
{
    public enum ECombatantType
    {
        Player,
        Rival
    }

    public ECombatantType combatantType;

    public HandController ownHand;
    public DeckController ownDeck;

    public int currentScore = 0;
    public int currentCombo = 0;

    private GameplayManager.PlayableSocket mySocket;

    protected virtual void Start()
    {
        mySocket = GameplayManager.GetBoardController().GetCardSocket(combatantType == ECombatantType.Player);
        ownHand.DrawCards(this);
    }

    public void PlayCard(CardController cardToPlay)
    {
        if(mySocket.IsSocketFree()) cardToPlay.CardPlayed(mySocket);
    }

    public bool CanPlayCard()
    {
        return GameplayManager.GetBoardController().CombatantCanPlayCard(combatantType == ECombatantType.Player);
    }

    public void RoundWin()
    {
        currentScore += (int)Mathf.Pow(2,currentCombo);
        currentCombo++;
        ModuleUI.UpdateScores();
    }

    public void RoundTied()
    {
        currentCombo = 0;
        currentScore += (int)Mathf.Pow(2, currentCombo);
        ModuleUI.UpdateScores();
    }

    public void RoundLost()
    {
        currentCombo = 0;
        ModuleUI.UpdateScores();
    }
}
