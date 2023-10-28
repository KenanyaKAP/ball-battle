using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierParameterPreset", menuName = "Soldier Parameter Preset")]
public class SoldierParameter : ScriptableObject {
    public float energyRegen;
    public float energyCost;
    public float spawnTime;
    public float reactiveTime;
    public float normalSpeed;
    public float carryingSpeed;
    public float returnSpeed;
    public float detectionRange;
    public float unitMultiplier;
}
