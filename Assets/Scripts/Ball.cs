using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [Header("Ball Variable")]
    [SerializeField] float ballSpeed = 1.5f;
    public GameObject isCarriedBy;
    
    public GameObject moveTarget;

    void Update() {
        if (moveTarget) {
            transform.Translate((moveTarget.transform.position - transform.position).normalized * ballSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);
        }
    }

    public void CarriedBy(GameObject who) {
        isCarriedBy = who;
        moveTarget = null;
    }

    public void SetGoTo(GameObject who) {
        moveTarget = who;
    }
}
