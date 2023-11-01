using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierParameterPreset", menuName = "Soldier Parameter Preset")]
public class SoldierParameter : ScriptableObject {
    public float spawnTime;
    public float reactiveTime;
    public float normalSpeed;
    public float carryingSpeed;
    public float returnSpeed;
    public float rotateSpeed;
    public float detectionRange;
}
