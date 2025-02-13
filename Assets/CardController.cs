using DG.Tweening;
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

    public PlayableSocket currentSocket;
    public bool isFaceDown = false;

    private UICardInputHandler cardInputHandler;

    private CanvasGroup canvasGroup;

    public CombatantController combatantOwner;

    public float flipDuration = 0.5f;
    public float scaleFactor = 1.1f;
    public GameObject cardFront;
    public GameObject cardBack;
    private bool flipingCard = false;

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
        if (flipingCard) return;

        Sequence flipSequence = DOTween.Sequence();
        flipingCard = true;
        float targetRotation = isFaceDown ? 0 : 180;
        // First half: Rotate to 90° and scale up together
        flipSequence.Append(transform.DOScale(scaleFactor, flipDuration / 2).SetEase(Ease.OutQuad))
                    .Join(transform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.Fast).SetEase(Ease.InOutQuad))
                    .OnStepComplete(() =>
                    {
                        Debug.Log("Flipeandoo");
                        // Swap front and back visuals
                        isFaceDown = !isFaceDown;
                        cardFront.SetActive(!isFaceDown);
                        cardBack.SetActive(isFaceDown);
                        flipSequence.Append(transform.DOScale(1f, flipDuration / 2).SetEase(Ease.OutQuad))
                       .Join(transform.DORotate(new Vector3(0, targetRotation, 0), flipDuration / 2, RotateMode.Fast).SetEase(Ease.InOutQuad))
                       .OnComplete(() =>
                       {
                           flipingCard = false;
                       });
                    });
        
        // Second half: Rotate to 180° and scale back together
        //flipSequence.Append(transform.DOScale(1f, flipDuration / 2).SetEase(Ease.OutQuad))
                        //.Join(transform.DORotate(new Vector3(0, 180, 0), flipDuration / 2, RotateMode.Fast).SetEase(Ease.InOutQuad));
        
    }

    public void CardPlayed(PlayableSocket socketToPlay)
    {
        SetCanBeMovedByInput(false);
        cardRectTransform.parent = socketToPlay.anchor;
        socketToPlay.playedCard = this;
        cardRectTransform.anchoredPosition = Vector2.zero;
        combatantOwner.ownHand.cardsInHand.Remove(this);
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
