using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandController : MonoBehaviour
{

    public int numberOfInitialCards = 3;

    public int horizontalMarginOffset = 40;

    public int minDistanceBetweenCards = 40;

    public GameObject anchorPrefab;

    private List<Transform> anchorTransforms;

    private RectTransform rect;

    private void Awake()
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
}
