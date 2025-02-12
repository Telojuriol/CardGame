using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalController : CombatantController
{

    public bool playCard = false;
    public int cardNumber = 1;

    private new void Start()
    {
        base.Start();
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

    public override void PlayCard(CardController cardToPlay)
    {
        base.PlayCard(cardToPlay);
        ownHand.RemoveCardFromHand(cardToPlay);
    }
}
