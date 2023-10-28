using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    void Start() {
        Application.targetFrameRate = 60;
    }
    void Update() {
        fpsText.text = string.Format("{0:F2} FPS", 1/Time.deltaTime);
    }
}
