using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalController : CombatantController
{

    public bool playCard = false;
    public int cardNumber = 1;

    public GameplayManager.EEnemyType enemyType;

    private new void Start()
    {
        base.Start();
    }

    private void OnEnable()
    {
        GameplayManager.waitingForCards += ManagerWaitingForCardToPlay;
    }

    private void OnDisable()
    {
        GameplayManager.waitingForCards -= ManagerWaitingForCardToPlay;
    }

    void Update()
    {
        if (playCard)
        {
            CardController cardToPlay = null;
            for (int i = 0; i < ownHand.cardsInHand.Count; i++)
            {
                if (ownHand.cardsInHand[i].cardNumber == cardNumber)
                {
                    cardToPlay = ownHand.cardsInHand[i];
                    PlayCard(cardToPlay);
                    break;
                }
            }
            playCard = false;
        }
    }

    private void ManagerWaitingForCardToPlay()
    {
        Debug.Log(ownHand.cardsInHand.Count);
        int cardIdToPlay = Random.Range(0,ownHand.cardsInHand.Count);
        PlayCard(ownHand.cardsInHand[cardIdToPlay]);
    }

    public void SetEnemyType(GameplayManager.EEnemyType enemyType)
    {
        this.enemyType = enemyType;
    }

    public override void PlayCard(CardController cardToPlay)
    {
        base.PlayCard(cardToPlay);
        ownHand.RemoveCardFromHand(cardToPlay);
    }
}
