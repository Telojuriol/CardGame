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
    private bool isCardSelected = false;
    private Vector2 desiredPosition;
    public float lerpSpeed = 10f;

    public float lerpSpeedWhenCardPressed = 9999999f;

    private float currentLerpSpeed = 10f;

    public bool cardPlayed = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        cardInputHandler = GetComponent<UICardInputHandler>();
    }

    private void Start()
    {
        InitializeCard();
        currentLerpSpeed = lerpSpeed;
    }
    private Vector2 previousPosition = Vector2.zero;
    private Vector2 movementDelta = Vector2.zero;

    private void Update()
    {
        if(!flipingCard) FollowRotation();

        if (!isCardSelected && currentSocket != null)
        {
            if (currentSocket.anchorRectTransform != null)
            {
                desiredPosition = ModuleUI.GetCanvasRectTransform().InverseTransformPoint(currentSocket.anchorRectTransform.position);
            }                     
        }
        cardRectTransform.anchoredPosition = Vector2.Lerp(cardRectTransform.anchoredPosition, desiredPosition, Time.deltaTime * lerpSpeed);
        movementDelta = cardRectTransform.anchoredPosition - previousPosition;
        previousPosition = cardRectTransform.anchoredPosition;
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
        bool facingUp = isFaceDown;
        bool alreadyFlipped = false;

        flipSequence.Append(cardRectTransform.DOScale(scaleFactor, flipDuration / 2).SetEase(Ease.OutQuad))
                    .Join(cardRectTransform.DORotate(new Vector3(0, 90, 0), flipDuration / 2, RotateMode.Fast).SetEase(Ease.InOutQuad))
                    .Append(cardRectTransform.DOScale(1f, flipDuration / 2).SetEase(Ease.OutQuad))
                    .Join(cardRectTransform.DORotate(new Vector3(0, targetRotation, 0), flipDuration / 2, RotateMode.Fast).SetEase(Ease.InOutQuad))
                    .OnUpdate(() =>
                    {
                        if (!alreadyFlipped)
                        {
                            bool flipCard = Mathf.Abs(90 - cardRectTransform.rotation.eulerAngles.y) < 3f;
                            if (flipCard)
                            {
                                isFaceDown = !isFaceDown;
                                cardFront.SetActive(!isFaceDown);
                                cardBack.SetActive(isFaceDown);
                                alreadyFlipped = true;
                            }
                        }
                    })
                    .OnStepComplete(() =>
                    {
                        flipingCard = false;
                    });
    }

    [SerializeField] private float rotationAmount = 20;
    [SerializeField] private float rotationSpeed = 20;
    [SerializeField] private float autoTiltAmount = 30;
    [SerializeField] private float manualTiltAmount = 20;
    [SerializeField] private float tiltSpeed = 20;
    private Vector3 rotationDelta;

    private void FollowRotation()
    {
        Vector3 movementRotation = movementDelta * rotationAmount;
        rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
        cardRectTransform.eulerAngles = new Vector3(0, 0, Mathf.Clamp(rotationDelta.x, -60, 60));
    }

    public void CardPlayed(PlayableSocket socketToPlay)
    {
        cardPlayed = true;
        SetCanBeMovedByInput(false);
        //cardRectTransform.parent = socketToPlay.anchor;
        socketToPlay.playedCard = this;
        currentSocket = socketToPlay;
        //cardRectTransform.anchoredPosition = Vector2.zero;
        if(!isFaceDown) SetCardFaceDownStatus(true);
        combatantOwner.ownHand.cardsInHand.Remove(this);
    }

    private void OnCardPressed()
    {
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
        //this.transform.parent = ModuleUI.GetCanvas().transform;
        combatantOwner.ownHand.RemoveCardFromHand(this);
        isCardSelected = true;
        currentLerpSpeed = lerpSpeedWhenCardPressed;
    }

    private void OnCardReleased()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        isCardSelected = false;
        currentLerpSpeed = lerpSpeed;
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
        desiredPosition += deltaMovement;
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

    public void SetCardFaceDownStatus(bool status)
    {
        isFaceDown = status;
        if (status)
        {
            
            cardFront.SetActive(false);
            cardBack.SetActive(true);       
        }
        else
        {
            cardFront.SetActive(true);
            cardBack.SetActive(false);
        }
    }

}
