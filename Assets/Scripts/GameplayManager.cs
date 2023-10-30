using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Header("Current Game State")]
    [SerializeField] int currentMatch;
    public bool isCurrentMatchRunning { get; private set; } = false;
    [SerializeField] float currentMatchTime = 0f;
    
    [Header("Current Player State")]
    [SerializeField] PlayerState Player1State = PlayerState.Attacker;
    [SerializeField] PlayerState Player2State = PlayerState.Defender;
    
    [Header("Game Assets")]
    [SerializeField] GameObject ballPrefab;

    void OnDestroy() {
        isCurrentMatchRunning = false;
    }

    void Start() {
        Invoke("Test", 3);
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

        if (isCurrentMatchRunning && currentMatchTime > 0) {
            currentMatchTime -= Time.deltaTime;
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
}
