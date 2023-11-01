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
    [SerializeField] GameObject carryingBallArrow;
    [SerializeField] SphereCollider detectCollider;
    
    [Header("Public Soldier Variable")]
    public WhichPlayer belongsTo = WhichPlayer.None;
    public bool isCarryingBall = false;

    [Header("Hiden Soldier Variable")]
    [SerializeField] bool isParameterSet = false;
    [SerializeField] SoldierParameter soldierParameter;
    [SerializeField] SoldierType type = SoldierType.Defender;
    [SerializeField] SoldierState currentState = SoldierState.Spawning;
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] Soldier targetAttacker;

    [Header("Material Related")]
    [Header("Soldier Renderer")]
    [SerializeField] Renderer head;
    [SerializeField] Renderer leftHand;
    [SerializeField] Renderer rightHand;
    [SerializeField] Renderer body;

    [Header("Gray Material")]
    [SerializeField] Material[] soldierGrayHead_Mats;
    [SerializeField] Material[] soldierGrayHand_Mats;
    [SerializeField] Material[] soldierGrayBody_Mats;
    
    [Header("Initial Material")]
    [SerializeField] Material[] initialSoldierHead_Mats;
    [SerializeField] Material[] initialSoldierHand_Mats;
    [SerializeField] Material[] initialSoldierBody_Mats;

    void Awake() {
        initialSoldierHead_Mats = head.materials;
        initialSoldierHand_Mats = leftHand.materials;
        initialSoldierBody_Mats = body.materials;
    }

    void Start() {
        if (!isParameterSet) {
            Debug.LogWarning("Warning! Soldier not yet set!");
        }
    }

    void Update()
    {
        if (!GameplayManager.instance.isCurrentMatchRunning) return;

        if (currentState == SoldierState.Inactive) {
            if (type == SoldierType.Defender) {
                GoToSpawnPosition();
            }
        }

        if (type == SoldierType.Attacker && currentState == SoldierState.Running) {
            // Attacker
            if (!GameplayManager.instance.gameBall.isCarriedBy) {
                ChaseBall();
            } else {
                if (isCarryingBall) {
                    GoToGoal();
                } else {
                    GoForward();
                }
            }

        } else if (type == SoldierType.Defender && currentState == SoldierState.Chasing) {
            ChaseAttacker();
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

    void OnTriggerStay(Collider other) {
        if (type == SoldierType.Defender && currentState == SoldierState.Defending) {
            Soldier attacker = other.GetComponent<Soldier>();
            if (attacker != null) {
                if (attacker.isCarryingBall) {
                    ChangeState(SoldierState.Chasing);
                    targetAttacker = attacker;
                }
            }
        }
    }

    // ================= Private Function =================
    void ChaseBall() {
        Vector3 moveDirection = GameplayManager.instance.gameBall.transform.position - transform.position;
        if (moveDirection.magnitude > 1.2f) {
            transform.Translate(moveDirection.normalized * soldierParameter.normalSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
        } else {
            GameplayManager.instance.gameBall.CarriedBy(gameObject);
            GameplayManager.instance.gameBall.transform.SetParent(transform);
            isCarryingBall = true;
            carryingBallArrow.SetActive(true);
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

        transform.Translate(goalDir.normalized * soldierParameter.carryingSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(goalDir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
    }
    
    void GoForward() {
        Vector3 targetDir = belongsTo == WhichPlayer.Player1 ? Vector3.right : Vector3.left;
        transform.Translate(targetDir * soldierParameter.normalSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);

        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
    }

    void ChaseAttacker() {
        Vector3 targetDir = targetAttacker.transform.position - transform.position;

        if (!targetAttacker.isCarryingBall) {
            ChangeState(SoldierState.Defending);
        }

        if (targetDir.magnitude > 1.2f) {
            transform.Translate(targetDir * soldierParameter.normalSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
        } else {
            targetAttacker.GetCaught();
            Inactive();
        }
    }

    void GetCaught() {
        PassBallIfAny();
        Inactive();
    }

    void PassBallIfAny() {
        List<Soldier> allys = belongsTo == WhichPlayer.Player1 ? GameplayManager.instance.player1Soldiers : GameplayManager.instance.player2Soldiers;

        float distance = float.PositiveInfinity;
        Soldier closestAlly = null;
        
        foreach (Soldier ally in allys) {
            if (ally == this || ally.currentState == SoldierState.Inactive) continue;

            if (Vector3.Distance(transform.position, ally.transform.position) < distance) {
                distance = Vector3.Distance(transform.position, ally.transform.position);
                closestAlly = ally;
            }
        }

        if (closestAlly) {
            // There is closest ally
            GameplayManager.instance.gameBall.CarriedBy(null);
            GameplayManager.instance.gameBall.transform.SetParent(null);
            isCarryingBall = false;
            carryingBallArrow.SetActive(false);
            
            GameplayManager.instance.gameBall.SetGoTo(closestAlly.gameObject);
        } else {
            GameplayManager.instance.EndMatch(belongsTo == WhichPlayer.Player1 ? WhichPlayer.Player2 : WhichPlayer.Player1);
            carryingBallArrow.SetActive(false);
            StopAllCoroutines();
        }
    }

    void Inactive() {
        ChangeState(SoldierState.Inactive);
        
        head.materials = soldierGrayHead_Mats;
        leftHand.materials = soldierGrayHand_Mats;
        rightHand.materials = soldierGrayHand_Mats;
        body.materials = soldierGrayBody_Mats;
        
        StartCoroutine(IEInactive());
    }

    IEnumerator IEInactive()
    {
        yield return new WaitForSeconds(soldierParameter.reactiveTime);

        if (GameplayManager.instance.isCurrentMatchRunning) {
            ResetSoldier();
        }
        yield return null;
    }

    void GoToSpawnPosition() {
        Vector3 targetDir = spawnPosition - transform.position;

        if (transform.position != spawnPosition) {
            transform.Translate(targetDir * soldierParameter.returnSpeed * Time.deltaTime * GameplayManager.instance.unityMultiplier, Space.World);

            Quaternion targetRotation = Quaternion.LookRotation(targetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, soldierParameter.rotateSpeed * Time.deltaTime);
        }
    }

    // ================= Essential Function =================
    void ChangeState(SoldierState toState) {
        currentState = toState;

        if (toState == SoldierState.Defending) {
            detectionArea.SetActive(true);
            directionArrow.SetActive(false);
        }
        
        if (toState == SoldierState.Running) {
            directionArrow.SetActive(true);
            detectionArea.SetActive(false);
        }

        if (toState == SoldierState.Chasing) {
            detectionArea.SetActive(false);
            directionArrow.SetActive(true);
        }

        if (toState == SoldierState.Inactive) {
            detectionArea.SetActive(false);
            directionArrow.SetActive(false);
        }
    }

    void ResetSoldier() {
        ChangeState(type == SoldierType.Attacker ? SoldierState.Running : SoldierState.Defending);
        targetAttacker = null;

        head.materials = initialSoldierHead_Mats;
        leftHand.materials = initialSoldierHand_Mats;
        rightHand.materials = initialSoldierHand_Mats;
        body.materials = initialSoldierBody_Mats;
    }
    
    // ================= Public Function =================
    public void Spawn(SoldierType type, WhichPlayer belongsTo) {
        isParameterSet = true;
        soldierParameter = type == SoldierType.Attacker ? attacker : defender;
        this.type = type;
        this.belongsTo = belongsTo;
        spawnPosition = transform.position;
        
        if (type == SoldierType.Defender) {
            detectCollider.radius = soldierParameter.detectionRange / 2f;
            detectionArea.transform.localScale = Vector3.one * soldierParameter.detectionRange / 6.65f;
        } else {
            Destroy(detectCollider.gameObject);
        }

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
