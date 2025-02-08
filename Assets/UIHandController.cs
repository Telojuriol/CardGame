using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandController : MonoBehaviour
{

    public int numberOfInitialCards = 3;

    public int horizontalMarginOffset = 40;

    public int minDistanceBetweenCards = 40;

    public bool DrawFromDeckOnStart = true;

    public GameObject anchorPrefab;

    private List<Transform> anchorTransforms;

    private RectTransform rect;

    private void Awake()
    {
        InitializeAnchors();
    }

    private void Start()
    {
        for(int i = 0; i < anchorTransforms.Count && GameplayManager.GetDeckController().GetRemainingCards() > 0; i++)
        {
            CardController newCard = GameplayManager.GetDeckController().DrawCardOnTop();
            if (newCard)
            {
                newCard.InitializeCard();
                newCard.transform.parent = anchorTransforms[i];
                newCard.cardRectTransform.anchoredPosition = Vector2.zero;
                newCard.cardRectTransform.localScale = Vector3.one;
            }
        }
    }

    private void InitializeAnchors()
    {
        anchorTransforms = new List<Transform>();
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
            anchorTransforms.Add(new_anchor.transform);
        }
    }

    public List<Transform> GetAnchorTransforms()
    {
        return anchorTransforms;
    }
}
