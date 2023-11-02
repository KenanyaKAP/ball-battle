using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeUI : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] TextMeshProUGUI timeText;

    void Start() {
        SetTime(0);
    }

    public void SetTime(float second) {
        int minutes = (int)second / 60;
        int seconds = (int)second % 60;

        timeText.text = $"{minutes}:{seconds:D2}";
    }
}
