using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Camera Position")]
    [SerializeField] Vector3 mainMenuPos;
    [SerializeField] Vector3 gameplayPos;

    [Header("Camera Position")]
    [SerializeField] float mainMenuOrthoSize = 40;
    [SerializeField] float gameplayOrthoSize = 27;

    [Header("Game Component")]
    [SerializeField] GameObject GameplayUI;
    [SerializeField] GameObject MainMenuUI;
    
    Camera mainCam;
    [SerializeField] bool needToMove;

    void Awake() {
        mainCam = Camera.main;
    }

    void Update() {
        if (needToMove) {
            transform.position = Vector3.Lerp(transform.position, gameplayPos, 2 * Time.deltaTime);
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, gameplayOrthoSize, 3 * Time.deltaTime);

            if ((transform.position - gameplayPos).magnitude < 1 && Mathf.Abs(mainCam.orthographicSize - gameplayOrthoSize) < .1f) {
                needToMove = false;
            }
        }
    }

    public void Play() {
        needToMove = true;

        GameplayUI.SetActive(true);
        MainMenuUI.SetActive(false);

        GameplayManager.instance.StartMatch();
    }
}
