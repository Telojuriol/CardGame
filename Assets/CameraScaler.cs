using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float targetAspect = 9f / 16f; // Adjust based on your intended aspect ratio

    void Start()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        Camera.main.orthographicSize = screenAspect > targetAspect
            ? 5f * (targetAspect / screenAspect)
            : 5f; // 5 is an example, adjust based on your sprites
    }
}
