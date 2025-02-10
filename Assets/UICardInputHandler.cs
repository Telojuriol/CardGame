using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICardInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition; // Store initial position
    private BoardController boardController;
    private RectTransform boardArea;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        boardController = GameplayManager.GetBoardController();
        boardArea = boardController.GetRectTransform();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition; // Save original position
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (IsOverBoard())
        {
            Debug.Log("Card placed on board!");
            SnapToBoard();
        }
        else
        {
            Debug.Log("Card returned to hand.");
            rectTransform.anchoredPosition = originalPosition; // Return to hand
        }
    }

    private bool IsOverBoard()
    {
        if (boardArea == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(boardArea, Input.mousePosition, canvas.worldCamera);
    }

    private void SnapToBoard()
    {
        rectTransform.SetParent(boardController.playableSockets[0].anchor, true); // Reparent to the board
        rectTransform.anchoredPosition = Vector2.zero; // Snap to center (adjust if needed)
    }

}