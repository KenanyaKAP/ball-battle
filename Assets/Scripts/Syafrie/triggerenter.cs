using UnityEngine;

public class triggerenter : MonoBehaviour
{
    public GameObject targetObject3; // GameObject yang akan bergerak
    public GameObject targetObject4; // GameObject yang menjadi target pergerakan
    public GameObject targetObject5;
    public float movementSpeed = 5.0f;

    private bool isColliding = false;

    void Update()
    {
        if (isColliding)
        {
            //Bola menuju ke spawn attacker, target 5 = attacker(clone)
            // Hitung arah menuju target
            Vector3 direction = targetObject3.transform.position - targetObject5.transform.position;

            
            direction.Normalize();

         
            targetObject3.transform.Translate(direction * movementSpeed * Time.deltaTime);
        }
    }

    //aku lupa collision enter kyknya gini, pokok intinya kalau si defender nabrak attacker, bola yg dibawa itu gerak ke attacker(clone)
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetObject4)
        {
            isColliding = true;
        }
    }

    
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == targetObject4)
        {
            isColliding = false;
        }
    }
}
