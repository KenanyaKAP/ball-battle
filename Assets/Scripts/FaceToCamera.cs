using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCamera : MonoBehaviour
{
    Camera mainCamera;

    void Awake(){
        mainCamera = Camera.main;
    }
 
    private void LateUpdate() {
        if (mainCamera != null) {
            transform.forward = -mainCamera.transform.forward;
        }
    }
}
