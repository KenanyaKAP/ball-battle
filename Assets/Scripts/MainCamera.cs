using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    [Header("Camera Position")]
    [SerializeField] Vector3 mainMenuPos;
    [SerializeField] Vector3 gameplayPos;

    [Header("Camera Position")]
    [SerializeField] float mainMenuOrthoSize = 40;
    [SerializeField] float middleOrthoSize = 30;
    [SerializeField] float gameplayOrthoSize = 27;

    [Header("Game Component")]
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MainMenuUI;

    [Header("Hiden Variable")]
    [SerializeField] bool isCamShaking;
    
    Camera mainCam;
    [SerializeField] bool needToMove;

    void Awake() {
        mainCam = Camera.main;
        instance = this;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ShakeCamera();
        }
    }

    public void Play() {
        LeanTween.value(0, 1, 1.5f).setOnUpdate((float value) => {
            transform.position = Vector3.Lerp(mainMenuPos, gameplayPos, value);
            mainCam.orthographicSize = Mathf.Lerp(mainMenuOrthoSize, gameplayOrthoSize, value);
        }).setEaseOutQuart();

        GameplayUI.SetActive(true);
        MainMenuUI.SetActive(false);

        GameplayManager.instance.StartMatch();
    }

    public void GoToMiddle() {
        float currentOrthoSize = mainCam.orthographicSize;
        LeanTween.value(0, 1, 1.5f).setOnUpdate((float value) => {
            mainCam.orthographicSize = Mathf.Lerp(currentOrthoSize, middleOrthoSize, value);
        }).setEaseOutQuart();
    }

    public void GoToGameplay() {
        float currentOrthoSize = mainCam.orthographicSize;
        LeanTween.value(0, 1, 1.5f).setOnUpdate((float value) => {
            mainCam.orthographicSize = Mathf.Lerp(currentOrthoSize, gameplayOrthoSize, value);
        }).setEaseOutQuart();
    }

    public void GoToMainMenu() {
        Vector3 currentPos = transform.position;
        float currentOrthoSize = mainCam.orthographicSize;
        LeanTween.value(0, 1, 1.5f).setOnUpdate((float value) => {
            transform.position = Vector3.Lerp(currentPos, mainMenuPos, value);
            mainCam.orthographicSize = Mathf.Lerp(currentOrthoSize, mainMenuOrthoSize, value);
        }).setEaseOutQuart();
    }

    public void ShakeCamera() {
        if (isCamShaking) return;

        isCamShaking = true;
        LeanTween.moveX(gameObject, transform.position.x + Random.Range(-.5f, .5f), 0.2f).setEaseShake();
        LeanTween.moveZ(gameObject, transform.position.z + Random.Range(-.5f, .5f), 0.2f).setEaseShake().setOnComplete(() => {
            LeanTween.moveX(gameObject, gameplayPos.x, 0.2f).setEaseShake();
            LeanTween.moveZ(gameObject, gameplayPos.z, 0.2f).setEaseShake().setOnComplete(() => {
                isCamShaking = false;
            });
        });
    }
}
