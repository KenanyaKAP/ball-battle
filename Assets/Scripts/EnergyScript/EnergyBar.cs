using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyBar : MonoBehaviour
{
    [Header("Hidden variable")]
    float energy;

    [Header("Energy Point")]
    [SerializeField] EnergyPoint point1;
    [SerializeField] EnergyPoint point2;
    [SerializeField] EnergyPoint point3;
    [SerializeField] EnergyPoint point4;
    [SerializeField] EnergyPoint point5;
    [SerializeField] EnergyPoint point6;
    
    [Header("Status")]
    [SerializeField] TextMeshProUGUI statusText;

    public void SetEnergy(float energy) {
        this.energy = energy;

        point1.SetValue(energy);
        point2.SetValue(energy-1);
        point3.SetValue(energy-2);
        point4.SetValue(energy-3);
        point5.SetValue(energy-4);
        point6.SetValue(energy-5);
    }

    public void SetStatusString(SoldierType type) {
        statusText.text = type.ToSoldierTypeString();
    }
}
