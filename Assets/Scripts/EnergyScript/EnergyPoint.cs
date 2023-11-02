using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPoint : MonoBehaviour
{   
    [Header("Component")]
    [SerializeField] Slider slider;
    [SerializeField] GameObject fullPoint;

    void Start() {
        slider.value = 0;
        fullPoint.SetActive(false);
    }

    public void SetValue(float value) {
        if (slider.value == value) return;
        
        slider.value = value;
        
        if (value >= 1 && !fullPoint.activeSelf) {
            fullPoint.SetActive(true);
        } 
        if (value < 1 && fullPoint.activeSelf) {
            fullPoint.SetActive(false);
        }
    }
}
