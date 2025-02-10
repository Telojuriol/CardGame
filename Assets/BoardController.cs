using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public List<PlayableSocket> playableSockets;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public RectTransform GetRectTransform()
    {
        return rectTransform;
    }

    public bool AllCardsPlayed()
    {
        bool allCardsPlayed = true;
        for(int i = 0; i < playableSockets.Count; i++)
        {
            allCardsPlayed = allCardsPlayed && playableSockets[i].playedCard != null;
        }
        return allCardsPlayed;
    }

    public Transform GetCardSocket(bool isPlayer)
    {
        if (isPlayer) return playableSockets[0].anchor;
        else return playableSockets[1].anchor;
    }

    public void CardPlayed(CardController playedCard, bool isPlayer)
    {
        if (isPlayer) playableSockets[0].playedCard = playedCard;
        else playableSockets[1].playedCard = playedCard;
    }

    [Serializable]
    public class PlayableSocket
    {
        public Transform anchor;
        [HideInInspector] public CardController playedCard;
    }
}
