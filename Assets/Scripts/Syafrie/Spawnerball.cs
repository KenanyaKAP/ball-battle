using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Spawnerball : MonoBehaviour
{
    private int xPos;
    private int zPos;
    public GameObject Prefabs;

    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop()
    {
        yield return new WaitForSeconds(0.5f);
        xPos = Random.Range(2, -1);
        zPos = Random.Range(-12, -7);
        Instantiate(Prefabs, new Vector3(xPos, 0.68f, zPos), Quaternion.identity);
        
    }

}
