using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{    
    [Header("Soldier Parameter Preset")]
    [SerializeField] SoldierParameter attacker;
    [SerializeField] SoldierParameter defender;
    
    [Header("Soldier Component")]
    [SerializeField] SpawnTimeCanvas spawnTimeCanvas;
    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject directionArrow;
    [SerializeField] GameObject detectionArea;
    
    [Header("Hiden Soldier Variable")]
    [SerializeField] bool isParameterSet = false;
    [SerializeField] SoldierParameter soldierParameter;
    [SerializeField] SoldierType type = SoldierType.Defender;
    public WhichPlayer belongsTo = WhichPlayer.None;
    [SerializeField] SoldierState currentState = SoldierState.Spawning;
    [SerializeField] bool isCarryingBall = false;

    void Start() {
        if (!isParameterSet) {
            Debug.LogWarning("Warning! Soldier not yet set!");
        }
    }

    void Update()
    {
        if (!GameplayManager.instance.isCurrentMatchRunning) return;

        if (type == SoldierType.Attacker && currentState == SoldierState.Running) {
            // Attacker
            if (!Ball.instance.isCarriedBy) {
                ChaseBall();
            } else {
                if (isCarryingBall) {
                    GoToGoal();
                } else {
                    GoForward();
                }
            }

        } else if (type == SoldierType.Defender) {
            // Defender
        }
    }

    void OnCollisionEnter(Collision other) {
        if (type == SoldierType.Attacker) {
            if (isCarryingBall) {
                if (other.transform.CompareTag(belongsTo == WhichPlayer.Player1 ? "Player2Goal" : "Player1Goal")) {
                    GameplayManager.instance.EndMatch(belongsTo);
                }
            } else {
                if (
                    other.transform.CompareTag(belongsTo == WhichPlayer.Player1 ? "Player2Goal" : "Player1Goal") ||
                    other.transform.CompareTag(belongsTo == WhichPlayer.Player1 ? "Player2Fench" : "Player1Fench")
                ) {
                    GameplayManager.instance.DestroySoldier(this);
                }
            }
        }
    }

    // ================= Private Function =================
    void ChaseBall() {
        Vector3 moveDirection = Ball.instance.transform.position - transform.position;
        if (moveDirection.magnitude > 1.2f) {
            transform.Translate(moveDirection.normalized * soldierParameter.normalSpeed * Time.deltaTime * soldierParameter.unitMultiplier, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
        } else {
            Ball.instance.isCarriedBy = gameObject;
            Ball.instance.transform.SetParent(transform);
            isCarryingBall = true;
        }
    }

    void GoToGoal() {
        Transform targetGoal = belongsTo == WhichPlayer.Player1 ? GameplayManager.instance.player2Goal.transform : GameplayManager.instance.player1Goal.transform;
        
        Vector3 goalPos = new Vector3(
            targetGoal.position.x, 
            0, 
            Mathf.Clamp(
                transform.position.z, 
                targetGoal.position.z - targetGoal.localScale.z/2, 
                targetGoal.position.z + targetGoal.localScale.z/2
            )
        );
        Vector3 goalDir = goalPos - transform.position;

        transform.Translate(goalDir.normalized * soldierParameter.carryingSpeed * Time.deltaTime * soldierParameter.unitMultiplier, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(goalDir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
    }
    
    void GoForward() {
        Vector3 targetDir = belongsTo == WhichPlayer.Player1 ? Vector3.right : Vector3.left;
        transform.Translate(targetDir * soldierParameter.normalSpeed * Time.deltaTime * soldierParameter.unitMultiplier, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
    }

    // ================= Essential Function =================
    void ChangeState(SoldierState toState) {
        currentState = toState;

        if (toState == SoldierState.Defending) {
            detectionArea.SetActive(true);
        }
        
        if (toState == SoldierState.Running) {
            directionArrow.SetActive(true);
        }
    }
    
    // ================= Public Function =================
    public void Spawn(SoldierType type, WhichPlayer belongsTo) {
        isParameterSet = true;
        soldierParameter = type == SoldierType.Attacker ? attacker : defender;
        this.type = type;
        this.belongsTo = belongsTo;

        ChangeState(SoldierState.Spawning);
        StartCoroutine(IESpawn());
    }

    IEnumerator IESpawn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < soldierParameter.spawnTime)
        {
            elapsedTime += Time.deltaTime;
            spawnTimeCanvas.SetTime(elapsedTime/soldierParameter.spawnTime);
            yield return null;
        }

        Destroy(spawnTimeCanvas.gameObject);

        if (type == SoldierType.Attacker) {
            ChangeState(SoldierState.Running);
        } else {
            ChangeState(SoldierState.Defending);
        }

        yield return null;
    }

}
