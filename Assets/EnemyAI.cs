 using UnityEngine;
 using System.Collections;
 
 public class EnemyAI : MonoBehaviour {
 
     public Transform player;
     public float playerDistance;
     public float rotationDamping;
     public float moveSpeed;
     public static bool isPlayerAlive = true;
 
     // Use this for initialization
     void Start () {
     
     }
     
     // Update is called once per frame
     void Update () {
 
         if (isPlayerAlive) {
             playerDistance = Vector3.Distance (player.position, transform.position);
 
             if (playerDistance < 5000f) {
                 lookAtPlayer ();
             }
             if (playerDistance < 4090f) {
                 if (playerDistance > 1.8f) {
                     chase ();
                 } else if (playerDistance < 1.8f) {
                     attack ();
                 }
             }
         }
     }
 
     void lookAtPlayer()
     {
         Quaternion rotation = Quaternion.LookRotation (player.position - transform.position);
         transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
     }
 
     void chase()
     {
         transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);
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
