using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    public int numberOfInitialCards = 3;

    public int currentNumberOnCardsOnHand = 0;

    public int horizontalMarginOffset = 40;

    public int minDistanceBetweenCards = 40;

    public bool DrawFromDeckOnStart = true;

    public GameObject anchorPrefab;

    public DeckController myDeck;

    private List<GameplayManager.PlayableSocket> anchorSockets;

    private RectTransform rect;

    public List<CardController> cardsInHand;

    private void Awake()
    {
        InitializeAnchors();

        cardsInHand = new List<CardController>();
    }

    public void DrawCards(CombatantController combatantOwner)
    {
        for (int i = 0; i < anchorSockets.Count && myDeck.GetRemainingCards() > 0; i++)
        {
            CardController newCard = myDeck.DrawCardOnTop();
            if (newCard)
            {
                newCard.InitializeCard();
                newCard.transform.parent = anchorSockets[i].anchor;
                newCard.cardRectTransform.anchoredPosition = Vector2.zero;
                newCard.cardRectTransform.localScale = Vector3.one;
                newCard.combatantOwner = combatantOwner;
                newCard.SetCanBeMovedByInput(combatantOwner.combatantType == CombatantController.ECombatantType.Player);
                newCard.currentSocket = anchorSockets[i];
                cardsInHand.Add(newCard);
            }
        }
    }

    public void DrawCard(CombatantController combatantOwner)
    {
        CardController newCard = myDeck.DrawCardOnTop();
        if (newCard)
        {
            newCard.InitializeCard();
            //newCard.transform.parent = anchorSockets[i].anchor;
            newCard.cardRectTransform.anchoredPosition = Vector2.zero;
            newCard.cardRectTransform.localScale = Vector3.one;
            newCard.combatantOwner = combatantOwner;
            newCard.SetCanBeMovedByInput(combatantOwner.combatantType == CombatantController.ECombatantType.Player);
            //newCard.currentSocket = anchorSockets[i];
            cardsInHand.Add(newCard);
        }
    }

    private void InitializeAnchors()
    {
        anchorSockets = new List<GameplayManager.PlayableSocket>();
        rect = GetComponent<RectTransform>();

        float handAnchorWidth = rect.rect.width - horizontalMarginOffset;

        // Determine the actual spacing between cards
        float actualAnchorDistance = Mathf.Min(handAnchorWidth / (numberOfInitialCards - 1), minDistanceBetweenCards);

        // Calculate total width occupied by the cards
        float totalCardWidth = actualAnchorDistance * (numberOfInitialCards - 1);

        // Center the anchors based on their total width
        float startX = -totalCardWidth / 2;

        for (int i = 0; i < numberOfInitialCards; i++)
        {
            GameObject new_anchor = Instantiate(anchorPrefab, transform);
            RectTransform new_trans = new_anchor.GetComponent<RectTransform>();

            new_trans.anchoredPosition = new Vector3(startX + i * actualAnchorDistance, 0);

            GameplayManager.PlayableSocket newSocket = new GameplayManager.PlayableSocket(new_anchor.transform);
            anchorSockets.Add(newSocket);
        }
    }

    public List<GameplayManager.PlayableSocket> GetAnchorTransforms()
    {
        return anchorSockets;
    }

}
