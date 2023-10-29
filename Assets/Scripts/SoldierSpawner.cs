using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SoldierSpawner : MonoBehaviour {

    [Header("Spawn Assets")]
    [SerializeField] GameObject soldierBlueTransparentPrefab;
    [SerializeField] GameObject soldierRedTransparentPrefab;
    [SerializeField] GameObject soldierBluePrefab;
    [SerializeField] GameObject soldierRedPrefab;

    [Header("Player 1 Spawn Parameter")]
    [SerializeField] Vector3 player1SpawnMinBounderies = new Vector3(-19.5f, 0f, -9.5f);
    [SerializeField] Vector3 player1SpawnMaxBounderies = new Vector3(0f, 0f, 9.5f);
    [SerializeField] Vector3 player1SpawnRotation = new Vector3(0f, 90f, 0f);
    
    [Header("Player 2 Spawn Parameter")]
    [SerializeField] Vector3 player2SpawnMinBounderies = new Vector3(1f, 0f, -9.5f);
    [SerializeField] Vector3 player2SpawnMaxBounderies = new Vector3(19.5f, 0f, 9.5f);
    [SerializeField] Vector3 player2SpawnRotation = new Vector3(0f, -90f, 0f);

    [Header("Private Variable")]
    Plane plane = new Plane(Vector3.down, 0);
    int player1FingerIndex = -1;
    int player2FingerIndex = -1;
    GameObject[] soldierGrayContainer = new GameObject[10];

    void Start() {
        Touch.onFingerDown += FingerDown;
        Touch.onFingerMove += FingerMove;
        Touch.onFingerUp += FingerUp;
    }

    void FingerDown(Finger finger) {
        if (GameplayManager.instance.isCurrentMatchRunning) {
            Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
            if (plane.Raycast(ray, out float distance)) {
                Vector3 worldTouchPos = Utils.SnapToGrid(ray.GetPoint(distance));

                if (worldTouchPos.x > 0) {
                    if (player2FingerIndex == -1) {
                        player2FingerIndex = finger.index;
                        soldierGrayContainer[finger.index] = Instantiate(soldierRedTransparentPrefab, worldTouchPos, Quaternion.Euler(player2SpawnRotation));
                    }
                } else {
                    if (player1FingerIndex == -1) {
                        player1FingerIndex = finger.index;
                        soldierGrayContainer[finger.index] = Instantiate(soldierBlueTransparentPrefab, worldTouchPos, Quaternion.Euler(player1SpawnRotation));
                    }
                }
            }
        }
    }

    void FingerMove(Finger finger) {
        if (soldierGrayContainer[finger.index] != null) {
            if (finger.index == player1FingerIndex) {
                Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
                if (plane.Raycast(ray, out float distance)) {
                    soldierGrayContainer[finger.index].transform.position = Utils.SnapToGrid(
                        Utils.Vector3Clamp(
                            ray.GetPoint(distance), player1SpawnMinBounderies, player1SpawnMaxBounderies
                        )
                    );
                }
            } else if (finger.index == player2FingerIndex) {
                Ray ray = Camera.main.ScreenPointToRay(finger.screenPosition);
                if (plane.Raycast(ray, out float distance)) {
                    soldierGrayContainer[finger.index].transform.position = Utils.SnapToGrid(
                        Utils.Vector3Clamp(
                            ray.GetPoint(distance), player2SpawnMinBounderies, player2SpawnMaxBounderies
                        )
                    );
                }
            }
        }
    }
    
    void FingerUp(Finger finger) {
        if (soldierGrayContainer[finger.index] != null) {
            Destroy(soldierGrayContainer[finger.index]);
            soldierGrayContainer[finger.index] = null;
        }

        if (finger.index == player1FingerIndex) {
            player1FingerIndex = -1;
        } else if (finger.index == player2FingerIndex) {
            player2FingerIndex = -1;
        } 
    }
}