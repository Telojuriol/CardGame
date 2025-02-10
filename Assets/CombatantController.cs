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
}
