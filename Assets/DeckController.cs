using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{

    public bool fillHandOnStart = true;

    public GameObject cardPrefab;

    public List<GameObject> deckCards;
    
    public bool shuffleOnStart = true;

    private void Start()
    {
        if (shuffleOnStart) Shuffle();
    }

    public CardController DrawCardOnTop()
    {
        GameObject drawnCard = deckCards.Count > 0 ? deckCards[0] : null;
        if (drawnCard)
        {
            GameObject newCard = Instantiate(deckCards[0].gameObject, ModuleUI.GetCanvas().transform);
            CardController newCardController = newCard.GetComponent<CardController>();
            deckCards.RemoveAt(0);
            return newCardController;
        }
        return null;
    }

    public void Shuffle()
    {
        System.Random rng = new System.Random();
        for (int i = deckCards.Count - 1; i > 0; i--)
        {
            int randomIndex = rng.Next(0, i + 1);
            (deckCards[i], deckCards[randomIndex]) = (deckCards[randomIndex], deckCards[i]);
        }
    }

    public int GetRemainingCards()
    {
        return deckCards.Count;
    }


}
