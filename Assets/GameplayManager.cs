using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public int initialHandCrads = 5;

    public static GameplayManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static int GetInitialHandCards()
    {
        return _instance.initialHandCrads;
    }
}
