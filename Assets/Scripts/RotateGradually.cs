using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGradually : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 25f;

    void Update() {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
