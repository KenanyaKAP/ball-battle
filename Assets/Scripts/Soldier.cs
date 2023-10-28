using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("Soldier Parameter")]
    [SerializeField] SoldierType soldierType = SoldierType.Defender;
    
    [Header("Hiden Soldier Parameter")]
    [SerializeField] bool isParameterSet = false;
    [SerializeField] SoldierParameter soldierParameter;
    
    [Header("Soldier Parameter Preset")]
    [SerializeField] SoldierParameter attacker;
    [SerializeField] SoldierParameter defender;
    
    void Start()
    {
        if (!isParameterSet) {
            Debug.LogWarning("Warning! Soldier not yet set!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(SoldierType type) {
        isParameterSet = true;
        soldierParameter = type == SoldierType.Attacker ? attacker : defender;
    }
}
