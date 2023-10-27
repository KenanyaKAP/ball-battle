using UnityEngine;

public class AttackerAI : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform goal;
    public float moveSpeed = 5.0f;
    public float ballCatchDistance = 1.0f;
    private int xPos;
    private int zPos;

    private bool hasBall = false;

    void Start()
    {
        Invoke("SpawnBall", 1.0f);
    }

    void SpawnBall()
    {
        xPos = Random.Range(2, -1);
        zPos = Random.Range(-12, -7);
        GameObject newBall = Instantiate(ballPrefab, new Vector3(xPos, 0.68f, zPos), Quaternion.identity);
        newBall.name = "Ball(Clone)";
    }

    void Update()
    {
        if (hasBall)
        {
            Vector3 goalDirection = goal.position - transform.position;
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
            
            GameObject ball = GameObject.Find("Ball(Clone)");
            if (ball != null)
            {
                Vector3 ballDirection = ball.transform.position - transform.position;
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
}
