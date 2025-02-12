using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public int cardNumber = 1;

    public TextMeshProUGUI cardText;
    public RectTransform cardRectTransform;

    private bool Initialized = false;

    public HandController handOwner;
    public PlayableSocket currentSocket;
    public bool isFaceDown = false;

    private UICardInputHandler cardInputHandler;

    private CanvasGroup canvasGroup;

    public CombatantController combatantOwner;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardInputHandler = GetComponent<UICardInputHandler>();
    }

    private void Start()
    {
        InitializeCard();
    }

    private void OnEnable()
    {
        cardInputHandler.onCardDragged += OnCardDragged;
        cardInputHandler.onCardPressed += OnCardPressed;
        cardInputHandler.onCardReleased += OnCardReleased;
    }

    private void OnDisable()
    {
        cardInputHandler.onCardDragged -= OnCardDragged;
        cardInputHandler.onCardPressed -= OnCardPressed;
        cardInputHandler.onCardReleased -= OnCardReleased;
    }

    public void InitializeCard()
    {
        if (!Initialized)
        {
            cardText.text = cardNumber.ToString();
            cardRectTransform = GetComponent<RectTransform>();
            //Initialized = true;
        }
    }

    public void TurnCard()
    {

    }

    public void CardPlayed(PlayableSocket socketToPlay)
    {
        SetCanBeMovedByInput(false);
        cardRectTransform.parent = socketToPlay.anchor;
        socketToPlay.playedCard = this;
        cardRectTransform.anchoredPosition = Vector2.zero;      
    }

    private void OnCardPressed()
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
        this.transform.parent = ModuleUI.GetCanvas().transform;
        combatantOwner.ownHand.RemoveCardFromHand(this);
    }

    private void OnCardReleased()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (IsOverBoard() && combatantOwner.CanPlayCard())
        {
            combatantOwner.PlayCard(this);
        }
        else
        {
            combatantOwner.ownHand.ReturnCardToHand(this);
        }
    }

    private void OnCardDragged(Vector2 deltaMovement)
    {
        cardRectTransform.anchoredPosition += deltaMovement;
    }

    private bool IsOverBoard()
    {
        RectTransform boardRectTransform = GameplayManager.GetBoardController().GetRectTransform();
        if (boardRectTransform == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(boardRectTransform, Input.mousePosition, ModuleUI.GetCanvas().worldCamera);
    }

    public void SetCanBeMovedByInput(bool status)
    {
        cardInputHandler.ActiveInput = status;
    }

}
