using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public List<PlayableSocket> playableSockets;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    [Serializable]
    public class PlayableSocket
    {
        public Transform anchor;
    }
}
