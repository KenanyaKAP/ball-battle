using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class GameplayManager : MonoBehaviour {
    // ================ Singleton ================
    public static GameplayManager instance;
    
    void Awake() {
        if (instance) {
            Debug.LogError("Cannot have GameplayManager more than 1!");
        } else {
            instance = this;
        }
    }
    // ================ Singleton ================

    [Header("Game Parameter")]
    [SerializeField] int gameMatch = 5;
    [SerializeField] float matchTimeLimit = 140f;
    [SerializeField] float energyRegen = 0.5f;

    [Header("Current Game State")]
    [SerializeField] int currentMatch;
    public bool isCurrentMatchRunning { get; private set; } = false;
    [SerializeField] float currentMatchTime = 0f;
    
    [Header("Current Player State")]
    [Header("Player 1")]
    public PlayerState player1State = PlayerState.Attacker;
    public float player1Energy = 0;
    
    [Header("Player 2")]
    public PlayerState player2State = PlayerState.Defender;
    public float player2Energy = 0;
    
    [Header("Game Component")]
    [SerializeField] EnergyBar player1EnergyBar;
    [SerializeField] EnergyBar player2EnergyBar;
    [SerializeField] TimeUI player1TimeUI;
    [SerializeField] TimeUI player2TimeUI;

    [Header("Game Assets")]
    public GameObject ballPrefab;
    public Transform goal;

    private int xPos;
    private int zPos;

    void OnDestroy() {
        isCurrentMatchRunning = false;
    }

    void Start() {
        Invoke("Test", 3);
        SpawnBall();
    }

    void Test() {
        StartMatch();
    }

    void Update() {
        // DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartMatch();
        }
        // DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY

        // Game is currently running
        if (isCurrentMatchRunning && currentMatchTime > 0) {
            // Time Related
            currentMatchTime -= Time.deltaTime;

            player1TimeUI.SetTime(currentMatchTime);
            player2TimeUI.SetTime(currentMatchTime);

            // Energy Related
            player1Energy += energyRegen * Time.deltaTime;
            player2Energy += energyRegen * Time.deltaTime;

            player1Energy = Mathf.Clamp(player1Energy, 0f, 6f);
            player2Energy = Mathf.Clamp(player2Energy, 0f, 6f);

            player1EnergyBar.SetEnergy(player1Energy);
            player2EnergyBar.SetEnergy(player2Energy);
        }
    }

    // =====================  Private Function =====================
    void StartMatch() {
        currentMatch = 0;
        currentMatchTime = matchTimeLimit;
        isCurrentMatchRunning = true;

        Debug.Log("Match Started!");
    }


    // ====================== Public Function ======================
    public void NextMatch() {
        if (currentMatch < gameMatch - 1) {
            // Next match
        } else {
            // End Game
        }
    }

    public void SpawnBall()
    {
        xPos = Random.Range(2, -1);
        zPos = Random.Range(-9, 9);
        GameObject newBall = Instantiate(ballPrefab, new Vector3(xPos, 0.04f, zPos), Quaternion.identity);
        newBall.name = "Ball(Clone)";
        
    }


}
