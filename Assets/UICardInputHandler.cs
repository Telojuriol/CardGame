using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static InputManager;
using static UICardInputHandler;

public class UICardInputHandler : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    public bool ActiveInput = true;

    public delegate void OnCardPressed();
    public delegate void OnCardReleased();
    public delegate void OnCardDragged(Vector2 deltaMovement);

    public event OnCardPressed onCardPressed;
    public event OnCardReleased onCardReleased;
    public event OnCardDragged onCardDragged;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!ActiveInput) return;
        onCardPressed?.Invoke();       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!ActiveInput || ModuleUI.GetCanvas() == null) return;
        onCardDragged?.Invoke(eventData.delta / ModuleUI.GetCanvas().scaleFactor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!ActiveInput) return;
        onCardReleased?.Invoke();
    }

}