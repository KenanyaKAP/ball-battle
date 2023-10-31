using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    public static Ball instance;

    [Header("Ball Variable")]
    public GameObject isCarriedBy;


    void Awake() {
        if (instance) {
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    public void CarryBall(GameObject who) {
        isCarriedBy = who;
    }
}
