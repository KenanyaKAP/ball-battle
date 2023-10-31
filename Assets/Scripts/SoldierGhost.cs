using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGhost : MonoBehaviour
{
    [SerializeField] GameObject detectionArea;
    public void SetType(SoldierType type) {
        if (type == SoldierType.Attacker) {
            Destroy(detectionArea);
        }
    }
}
