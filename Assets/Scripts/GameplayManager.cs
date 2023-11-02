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
    public float unityMultiplier = 3f;

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
    public Ball gameBall;
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
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject blueWinParticle;
    [SerializeField] GameObject redWinParticle;

    void OnDestroy() {
        isCurrentMatchRunning = false;
    }

    void Update() {
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

        if (isCurrentMatchRunning && currentMatchTime <= 0) {
            EndMatch(WhichPlayer.None);
        }
    }

    // =====================  Private Function =====================
    void NextMatch() {
        currentMatch += 1;

        if (currentMatch < gameMatch) {
            // Camera
            MainCamera.instance.GoToGameplay();
            
            // Switch Role
            player1Type = player1Type == SoldierType.Attacker ? SoldierType.Defender : SoldierType.Attacker;
            player2Type = player2Type == SoldierType.Attacker ? SoldierType.Defender : SoldierType.Attacker;

            // Set text
            matchText.text = (currentMatch + 1).ToString();
            player1WorldTypeText.text = player1Type.ToSoldierTypeString();
            player2WorldTypeText.text = player2Type.ToSoldierTypeString();
            player1EnergyBar.SetStatusString(player1Type);
            player2EnergyBar.SetStatusString(player2Type);

            // Clean All Soldier
            foreach (Soldier soldier in player1Soldiers) {
                soldier.DestroyEffect();
                Destroy(soldier.gameObject);
            }
            player1Soldiers.Clear();
            foreach (Soldier soldier in player2Soldiers) {
                soldier.DestroyEffect();
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
            WhichPlayer winner = player1Score == player2Score ? WhichPlayer.None : (player1Score >= player2Score ? WhichPlayer.Player1 : WhichPlayer.Player2);
            GameplayUISpawner.instance.EndGame();
            EndGameCanvasUI.instance.EndGame(winner);
            MainCamera.instance.GoToMainMenu();

            // Clean All Soldier
            foreach (Soldier soldier in player1Soldiers) {
                soldier.DestroyEffect();
                Destroy(soldier.gameObject);
            }
            player1Soldiers.Clear();
            foreach (Soldier soldier in player2Soldiers) {
                soldier.DestroyEffect();
                Destroy(soldier.gameObject);
            }
            player2Soldiers.Clear();
            
            if (winner == WhichPlayer.Player1) {
                GameObject winParticle = Instantiate(blueWinParticle, Vector3.right * -20f, Quaternion.identity);
                winParticle.transform.localScale = Vector3.one * 5f;
            } else if (winner == WhichPlayer.Player2) {
                GameObject winParticle = Instantiate(redWinParticle, Vector3.right * -20f, Quaternion.identity);
                winParticle.transform.localScale = Vector3.one * 5f;
            } 
        }
    }

    void SpawnBall(bool toArenaPlayer1) {
        if (gameBall) {
            Destroy(gameBall.gameObject);
        }

        if (toArenaPlayer1) {
            gameBall = Instantiate(ballPrefab, Utils.SnapToGrid(
                Utils.RandomRangeVector3(
                    soldierSpawner.player1SpawnMinBounderies, soldierSpawner.player1SpawnMaxBounderies
                )
            ), Quaternion.identity).GetComponent<Ball>();
        } else {
            gameBall = Instantiate(ballPrefab, Utils.SnapToGrid(
                Utils.RandomRangeVector3(
                    soldierSpawner.player2SpawnMinBounderies, soldierSpawner.player2SpawnMaxBounderies
                )
            ), Quaternion.identity).GetComponent<Ball>();
        }
    }

    // ====================== Public Function ======================
    public void StartMatch() {
        // Resetting All Variable
        currentMatch = 0;
        currentMatchTime = matchTimeLimit;
        isCurrentMatchRunning = true;
        
        player1Score = 0;
        player2Score = 0;
        
        player1Type = SoldierType.Attacker;
        player1Energy = 0;
        player1Soldiers.Clear();
        
        player2Type = SoldierType.Defender;
        player2Energy = 0;
        player2Soldiers.Clear();

        SpawnBall(true);

        matchText.text = (currentMatch + 1).ToString();
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();

        Debug.Log("Match Started!");
    }
    
    public void EndMatch(WhichPlayer winner) {
        isCurrentMatchRunning = false;
        Debug.Log("End Match, the winner is " + winner.ToString());

        MainCamera.instance.ShakeCamera();
        MainCamera.instance.GoToMiddle();

        GameplayUISpawner.instance.PopUpMatch(winner);

        if (winner == WhichPlayer.Player1) {
            player1Score += 1;
            player1ScoreText.text = player1Score.ToString();
        } else if (winner == WhichPlayer.Player2) {
            player2Score += 1;
            player2ScoreText.text = player2Score.ToString();
        } else if (winner == WhichPlayer.None) {

        }

        // Stop Soldier Animation
        foreach (Soldier soldier in player1Soldiers) {
            soldier.animController.StopAnimation();
        }
        foreach (Soldier soldier in player2Soldiers) {
            soldier.animController.StopAnimation();
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
            int index = player1Soldiers.IndexOf(soldier);
            if (index != -1) {
                player1Soldiers.RemoveAt(index);
            }
        } else if (soldier.belongsTo == WhichPlayer.Player2) {
            int index = player2Soldiers.IndexOf(soldier);
            if (index != -1) {
                player2Soldiers.RemoveAt(index);
            }
        }

        Destroy(soldier.gameObject);
    }

    public void Exit() {
        Application.Quit();
    }
}
