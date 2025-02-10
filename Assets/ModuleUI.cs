using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModuleUI : MonoBehaviour
{
    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI rivalScore;

    public Canvas UICanvas;

    public static ModuleUI _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Canvas GetCanvas()
    {
        return _instance.UICanvas;
    }
}
