using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    private float cardsNumberOnHand = 3;

    void Start()
    {
        cardsNumberOnHand = GameplayManager.GetInitialHandCards();

    }

    void Update()
    {
        
    }
}
