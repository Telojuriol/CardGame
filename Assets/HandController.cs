using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public int currentNumberOfCardsOnHand = 0;

    public int horizontalMarginOffset = 40;

    public int minDistanceBetweenCards = 40;

    public bool DrawFromDeckOnStart = true;

    public GameObject anchorPrefab;

    public DeckController myDeck;

    public List<PlayableSocket> anchorSockets;

    private RectTransform rect;

    public List<CardController> cardsInHand;

    private void Awake()
    {
        //UpdateAnchors();
        anchorSockets = new List<PlayableSocket> ();
        cardsInHand = new List<CardController>();
    }

    public void DrawCard(CombatantController combatantOwner)
    {
        CardController newCard = myDeck.DrawCardOnTop();
        if (newCard)
        {
            newCard.InitializeCard();
            AddCardToHand(newCard);       
            newCard.combatantOwner = combatantOwner;
            newCard.SetCanBeMovedByInput(combatantOwner.combatantType == CombatantController.ECombatantType.Player);
            cardsInHand.Add(newCard);
        }
    }

    public void RemoveCardFromHand(CardController cardToRemove)
    {
        for(int i = 0; i < anchorSockets.Count; i++)
        {
            if (anchorSockets[i].playedCard == cardToRemove)
            {
                anchorSockets[i].playedCard = null;
                break;
            }
        }
        RemoveAnchorFromHand();
    }

    public void ReturnCardToHand(CardController cardToReturn)
    {
        AddCardToHand(cardToReturn);
    }

    public void RemoveAnchorFromHand()
    {       
        PlayableSocket socketToRemove = GetFirstFreeSocket();
        if(socketToRemove != null)
        {
            currentNumberOfCardsOnHand--;
            RemoveSocketFromList(socketToRemove);
            GetAnchorTransforms();
            UpdateAnchors();
        }        
    }

    private void AddCardToHand(CardController cardToAdd)
    {
        currentNumberOfCardsOnHand++;
        GameObject new_anchor = Instantiate(anchorPrefab, transform);
        PlayableSocket newSocket = new PlayableSocket(new_anchor.transform);
        anchorSockets.Add(newSocket);
        
        cardToAdd.transform.parent = newSocket.anchor;
        cardToAdd.cardRectTransform.anchoredPosition = Vector2.zero;
        cardToAdd.cardRectTransform.localScale = Vector3.one;
        cardToAdd.currentSocket = newSocket;
        newSocket.playedCard = cardToAdd;

        UpdateAnchors();
    }

    public PlayableSocket GetFirstFreeSocket()
    {
        PlayableSocket socketToReturn = null;

        for (int i = 0; i < anchorSockets.Count; i++)
        {
            if (anchorSockets[i].playedCard == null)
            {
                socketToReturn = anchorSockets[i];
                break;
            }
        }

        return socketToReturn;
    }

    private void RemoveSocketFromList(PlayableSocket socketToRemove)
    {
        Destroy(socketToRemove.anchor.gameObject);
        anchorSockets.Remove(socketToRemove);
    }

    private void UpdateAnchors()
    {
        rect = GetComponent<RectTransform>();

        float handAnchorWidth = rect.rect.width - horizontalMarginOffset;

        // Determine the actual spacing between cards
        float actualAnchorDistance = Mathf.Min(handAnchorWidth / (currentNumberOfCardsOnHand - 1), minDistanceBetweenCards);

        // Calculate total width occupied by the cards
        float totalCardWidth = actualAnchorDistance * (currentNumberOfCardsOnHand - 1);

        // Center the anchors based on their total width
        float startX = -totalCardWidth / 2;

        for (int i = 0; i < anchorSockets.Count; i++)
        {
            RectTransform new_trans = anchorSockets[i].anchor.GetComponent<RectTransform>();

            new_trans.anchoredPosition = new Vector3(startX + i * actualAnchorDistance, 0);
            anchorSockets[i].playedCard.cardRectTransform.anchoredPosition = Vector2.zero;
        }
    }

    public List<PlayableSocket> GetAnchorTransforms()
    {
        return anchorSockets;
    }

}


