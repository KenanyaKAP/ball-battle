using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public GameObject Musuh;
        public int xPos;
        public int zPos;
        public int EnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        while (EnemyCount < 10)
        {
            xPos = Random.Range(272, 238);
            zPos = Random.Range(272, 286);
            Instantiate(Musuh, new Vector3(xPos, 1, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            EnemyCount += 1;
        }
                        
    }
}
