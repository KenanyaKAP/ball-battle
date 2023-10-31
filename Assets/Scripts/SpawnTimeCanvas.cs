using UnityEngine;
using UnityEngine.UI;

public class SpawnTimeCanvas : MonoBehaviour
{
    Camera mainCamera;
    [SerializeField] Slider slider;
    
    void Awake(){
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        if (mainCamera != null) {
            transform.forward = -mainCamera.transform.forward;
        }
    }

    public void SetTime(float value) {
        slider.value = value;
    }
}
