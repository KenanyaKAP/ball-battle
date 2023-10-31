using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] int player1Score;
    [SerializeField] int player2Score;
    public bool isCurrentMatchRunning { get; private set; } = false;
    [SerializeField] float currentMatchTime = 0f;
    
    [Header("Current Player State")]
    [Header("Player 1")]
    public SoldierType player1Type = SoldierType.Attacker;
    public float player1Energy = 0;
    public List<Soldier> player1Soldiers = new List<Soldier>();
    
    [Header("Player 2")]
    public SoldierType player2Type = SoldierType.Defender;
    public float player2Energy = 0;
    public List<Soldier> player2Soldiers = new List<Soldier>();
    
    [Header("Game Component")]
    [SerializeField] SoldierSpawner soldierSpawner;
    [SerializeField] EnergyBar player1EnergyBar;
    [SerializeField] EnergyBar player2EnergyBar;
    [SerializeField] TimeUI player1TimeUI;
    [SerializeField] TimeUI player2TimeUI;
    [SerializeField] TextMeshProUGUI matchText;
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [SerializeField] TextMeshProUGUI player1WorldTypeText;
    [SerializeField] TextMeshProUGUI player2WorldTypeText;

    [Header("Goal Component")]
    public GameObject player1Goal;
    public GameObject player2Goal;

    [Header("Game Assets")]
    public GameObject ballPrefab;
    public Transform goal;

    void OnDestroy() {
        isCurrentMatchRunning = false;
    }

    // DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY
    void Start() {
        // Invoke("StartMatch", 3);
    }
    // DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY DEBUG ONLY

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
    void NextMatch() {
        currentMatch += 1;

        if (currentMatch < gameMatch) {
            matchText.text = (currentMatch + 1).ToString();
            
            // Switch Role
            player1Type = player1Type == SoldierType.Attacker ? SoldierType.Defender : SoldierType.Attacker;
            player2Type = player2Type == SoldierType.Attacker ? SoldierType.Defender : SoldierType.Attacker;

            // Set text
            player1WorldTypeText.text = player1Type.ToSoldierTypeString();
            player2WorldTypeText.text = player2Type.ToSoldierTypeString();
            player1EnergyBar.SetStatusString(player1Type);
            player2EnergyBar.SetStatusString(player2Type);

            // Clean All Soldier
            foreach (Soldier soldier in player1Soldiers) {
                Destroy(soldier.gameObject);
            }
            player1Soldiers.Clear();
            foreach (Soldier soldier in player2Soldiers) {
                Destroy(soldier.gameObject);
            }
            player2Soldiers.Clear();

            // Reset Variable
            player1Energy = 0;
            player2Energy = 0;
            currentMatchTime = matchTimeLimit;
            isCurrentMatchRunning = true;

            SpawnBall(player1Type == SoldierType.Attacker);
        } else {
            // End Game
        }
    }

    void SpawnBall(bool toArenaPlayer1) {
        if (toArenaPlayer1) {
            Instantiate(ballPrefab, Utils.SnapToGrid(
                Utils.RandomRangeVector3(
                    soldierSpawner.player1SpawnMinBounderies, soldierSpawner.player1SpawnMaxBounderies
                )
            ), Quaternion.identity);
        } else {
            Instantiate(ballPrefab, Utils.SnapToGrid(
                Utils.RandomRangeVector3(
                    soldierSpawner.player2SpawnMinBounderies, soldierSpawner.player2SpawnMaxBounderies
                )
            ), Quaternion.identity);
        }
    }

    // ====================== Public Function ======================
    public void StartMatch() {
        currentMatch = 0;
        currentMatchTime = matchTimeLimit;
        isCurrentMatchRunning = true;

        SpawnBall(true);

        matchText.text = (currentMatch + 1).ToString();
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();

        Debug.Log("Match Started!");
    }
    
    public void EndMatch(WhichPlayer winner) {
        isCurrentMatchRunning = false;

        if (winner == WhichPlayer.Player1) {
            player1Score += 1;
            player1ScoreText.text = player1Score.ToString();
        } else if (winner == WhichPlayer.Player2) {
            player2Score += 1;
            player2ScoreText.text = player2Score.ToString();
        }

        StartCoroutine(IEEndMatch());
    }

    IEnumerator IEEndMatch() {
        yield return new WaitForSeconds(3f);
        NextMatch();
        yield return null;
    }

    public void DestroySoldier(Soldier soldier) {
        if (soldier.belongsTo == WhichPlayer.Player1) {
            player1Soldiers.Remove(soldier);
        } else if (soldier.belongsTo == WhichPlayer.Player2) {
            player2Soldiers.Remove(soldier);
        }

        Destroy(soldier.gameObject);
    }
}
