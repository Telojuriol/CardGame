using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivalController : MonoBehaviour
{

    public bool playCard = false;
    public int cardNumber = 1;

    public UIHandController hand;

    void Start()
    {
        
    }

    void Update()
    {
        if (playCard)
        {
            CardController cardToPlay = null;
            for (int i = 0; i < hand.cardsInHand.Count; i++)
            {
                if (hand.cardsInHand[i].cardNumber == cardNumber)
                {
                    cardToPlay = hand.cardsInHand[i];
                    PlayCard(cardToPlay);
                    break;
                }
            }
            playCard = false;
        }
    }

    public void PlayCard(CardController cardToPlay)
    {
        hand.PlayCard(cardToPlay, true);
    }
}
