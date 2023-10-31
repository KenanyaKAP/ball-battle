using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SoldierSpawner : MonoBehaviour {

    [Header("Spawn Assets")]
    [SerializeField] GameObject soldierGhostBluePrefab;
    [SerializeField] GameObject soldierGhostRedPrefab;
    [SerializeField] GameObject soldierBluePrefab;
    [SerializeField] GameObject soldierRedPrefab;

    [Header("Player 1 Spawn Parameter")]
    public Vector3 player1SpawnMinBounderies = new Vector3(-19.5f, 0f, -9.5f);
    public Vector3 player1SpawnMaxBounderies = new Vector3(0f, 0f, 9.5f);
    [SerializeField] Vector3 player1SpawnRotation = new Vector3(0f, 90f, 0f);
    
    [Header("Player 2 Spawn Parameter")]
    public Vector3 player2SpawnMinBounderies = new Vector3(1f, 0f, -9.5f);
    public Vector3 player2SpawnMaxBounderies = new Vector3(19.5f, 0f, 9.5f);
    [SerializeField] Vector3 player2SpawnRotation = new Vector3(0f, -90f, 0f);
    
    [Header("Spawn Parameter")]
    [SerializeField] int attackerEnergyCost = 2;
    [SerializeField] int defenderEnergyCost = 3;

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
                Vector3 worldTouchPos = ray.GetPoint(distance);

                if (worldTouchPos.x > 0) {
                    if (player2FingerIndex == -1) {
                        worldTouchPos = Utils.SnapToGrid(Utils.Vector3Clamp(
                            worldTouchPos, player2SpawnMinBounderies, player2SpawnMaxBounderies
                        ));
                        player2FingerIndex = finger.index;
                        soldierGrayContainer[finger.index] = Instantiate(soldierGhostRedPrefab, worldTouchPos, Quaternion.Euler(player2SpawnRotation));
                        soldierGrayContainer[finger.index].GetComponent<SoldierGhost>().SetType(GameplayManager.instance.player2Type);
                    }
                } else {
                    if (player1FingerIndex == -1) {
                        worldTouchPos = Utils.SnapToGrid(Utils.Vector3Clamp(
                            worldTouchPos, player1SpawnMinBounderies, player1SpawnMaxBounderies
                        ));
                        player1FingerIndex = finger.index;
                        soldierGrayContainer[finger.index] = Instantiate(soldierGhostBluePrefab, worldTouchPos, Quaternion.Euler(player1SpawnRotation));
                        soldierGrayContainer[finger.index].GetComponent<SoldierGhost>().SetType(GameplayManager.instance.player1Type);
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
            if (finger.index == player1FingerIndex)
            {
                player1FingerIndex = -1;
                int cost = GameplayManager.instance.player1Type == SoldierType.Attacker ? attackerEnergyCost : defenderEnergyCost;
                if (GameplayManager.instance.player1Energy >= cost)
                {
                    Soldier Attacker = Instantiate(
                        soldierBluePrefab, 
                        soldierGrayContainer[finger.index].transform.position,
                        soldierGrayContainer[finger.index].transform.rotation
                        ).GetComponent<Soldier>();
                    Attacker.Spawn(GameplayManager.instance.player1Type, WhichPlayer.Player1);
                    
                    GameplayManager.instance.player1Energy = GameplayManager.instance.player1Energy - cost;
                    GameplayManager.instance.player1Soldiers.Add(Attacker);
                }
            }

            else if (finger.index == player2FingerIndex)
            {
                player2FingerIndex = -1;
                int cost = GameplayManager.instance.player2Type == SoldierType.Attacker ? attackerEnergyCost : defenderEnergyCost;
                if (GameplayManager.instance.player2Energy >= cost)
                {
                    Soldier Defender = Instantiate(
                        soldierRedPrefab, 
                        soldierGrayContainer[finger.index].transform.position,
                        soldierGrayContainer[finger.index].transform.rotation
                        ).GetComponent<Soldier>();
                    Defender.Spawn(GameplayManager.instance.player2Type, WhichPlayer.Player2);
                    
                    GameplayManager.instance.player2Energy = GameplayManager.instance.player2Energy - cost;
                    GameplayManager.instance.player2Soldiers.Add(Defender);
                }
            }

            Destroy(soldierGrayContainer[finger.index]);
            soldierGrayContainer[finger.index] = null;
        } 
    }
}