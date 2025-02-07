using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private CardController holdedCard;

    private void OnEnable()
    {
        InputManager.onTapPerformed += OnTapPressed;
        InputManager.onHoldStarted += OnHoldStarted;
        InputManager.onHoldEnded += OnHoldEnded;
    }

    private void OnDisable()
    {
        InputManager.onTapPerformed -= OnTapPressed;
        InputManager.onHoldStarted -= OnHoldStarted;
        InputManager.onHoldEnded -= OnHoldEnded;
    }

    private void OnTapPressed(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if (hit.collider)
        {
            Debug.Log("Tap on " + hit.collider.gameObject.name);
        }
        //card.transform.position = position;
    }

    private void OnHoldStarted(Vector2 holdPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(holdPosition, Vector2.zero);
        if(hit)
        {
            CardController new_holdedCard = hit.collider.gameObject.GetComponent<CardController>();
            if (new_holdedCard)
            {
                holdedCard = new_holdedCard;
                holdedCard.HoldCard();
            }
        }
    }

    private void OnHoldEnded()
    {
        if (holdedCard)
        {
            holdedCard.UnholdCard();
        }
        holdedCard = null;
    }

    private void Update()
    {
        if (holdedCard)
        {
            holdedCard.UpdateDesiredPosition(InputManager.GetHoldPosition());
        }
    }

}
