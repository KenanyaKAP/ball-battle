using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    [Header("Soldier Parameter")]
    [SerializeField] SoldierType soldierType = SoldierType.Defender;
    
    [Header("Hiden Soldier Parameter")]
    [SerializeField] bool isParameterSet = false;
    [SerializeField] SoldierParameter soldierParameter;
    
    [Header("Soldier Parameter Preset")]
    [SerializeField] SoldierParameter attacker;
    [SerializeField] SoldierParameter defender;

    [Header("Attacker Soldier Parameter")]
    public float moveSpeed = 5.0f;
    public float ballCatchDistance = 1.0f;
    private bool hasBall = false;

    [Header("Defender Soldier Parameter")]
    [SerializeField] Transform player;
    [SerializeField] float playerDistance;
    [SerializeField] float rotationDamping;
    [SerializeField] float moveSpeedDef;
    [SerializeField] static bool isPlayerAlive = true;

    Vector3 ballDirection;

    void Start()
    {
        if (!isParameterSet) {
            Debug.LogWarning("Warning! Soldier not yet set!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(ballDirection * moveSpeed * Time.deltaTime);
    }

    public void Spawn(SoldierType type) {
        isParameterSet = true;
        soldierParameter = type == SoldierType.Attacker ? attacker : defender;
        if (soldierParameter == attacker)
        {
            Attacker();  
        }
        else if (soldierParameter == defender)
        {
            //Defender();
        }
    }

    void Attacker()
    {
        if (hasBall)
        {
            Vector3 goalDirection = GameplayManager.instance.goal.position - transform.position;
            float distanceToGoal = goalDirection.magnitude;

            if (distanceToGoal <= 1.0f)
            {

                Debug.Log("Gol!");

            }
            else
            {
                goalDirection.Normalize();
                transform.Translate(goalDirection * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            GameObject ball = Ball.instance.gameObject;
            if (ball != null)
            {
                Debug.Log("cari bola");
                ballDirection = ball.transform.position - transform.position;
                ballDirection.Normalize();
                float distanceToBall = ballDirection.magnitude;

                if (distanceToBall <= ballCatchDistance)
                {

                    hasBall = true;
                    ball.transform.parent = transform;
                }
                else
                {

                    ballDirection.Normalize();
                    transform.Translate(ballDirection * moveSpeed * Time.deltaTime);
                }
            }
        }
    }

    void Defender()
    {
        if (isPlayerAlive)
        {
            playerDistance = Vector3.Distance(player.position, transform.position);

            if (playerDistance < 5000f)
            {
                lookAtPlayer();
            }
            if (playerDistance < 4090f)
            {
                if (playerDistance > 1.8f)
                {
                    chase();
                }
                else if (playerDistance < 1.8f)
                {
                    attack();
                }
            }
        }
    }

    void lookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
    }

    void chase()
    {
        transform.Translate(Vector3.forward * moveSpeedDef * Time.deltaTime);
    }

    void attack()
    {
        //Ketika defender attack, attacker akan berhenti dan bola berjalan menuju attacker lain. Setelah sampai, bola berhenti


        // RaycastHit hit;
        // if (Physics.Raycast (transform.position, transform.forward, out hit))
        // {
        //   if(hit.collider.gameObject.tag == "Player")
        // {
        //   hit.collider.gameObject.GetComponent<PlayerHealth>().health -= 5f;
        // }
        // }
    }
}
