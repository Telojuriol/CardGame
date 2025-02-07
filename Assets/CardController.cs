using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{

    private bool isHeldByPlayer = false;

    private Vector2 desiredDestination;

    public float speedToDesiredPosition = 10f;

    private void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, desiredDestination, Time.deltaTime * speedToDesiredPosition);
    }

    public void HoldCard()
    {
        Debug.Log("Hooold");
        isHeldByPlayer = true;
        transform.localScale = Vector3.one * 1.1f;
    }

    public void UnholdCard()
    {
        isHeldByPlayer = false;
        //desiredDestination = Vector2.zero;
        transform.localScale = Vector3.one;
    }

    public void UpdateDesiredPosition(Vector2 new_position)
    {
        desiredDestination = new_position;
    }
}
