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

    public UIHandController handOwner;
    public bool isFaceDown = false;

    private void Start()
    {
        InitializeCard();
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

}
