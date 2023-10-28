using System;
using UnityEngine;

public class SoldierSpawner : MonoBehaviour {
    [Header("DEBUG ONLY")]
    [SerializeField] GameObject soldier;

    [Header("Spawn Assets")]
    [SerializeField] GameObject soldierPrefabs;

    [Header("Spawn Parameter")]
    [SerializeField] Vector3 spawnMinBounderies = new Vector3(-10f, 0f, -10f);
    [SerializeField] Vector3 spawnMaxBounderies = new Vector3(10f, 0f, 10f);

    Plane plane = new Plane(Vector3.down, 0);

    void Update() {
        if (GameplayManager.instance.isCurrentMatchRunning) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out float distance)) {
                soldier.transform.position = Utils.SnapToGrid(
                    Utils.Vector3Clamp(
                        ray.GetPoint(distance), spawnMinBounderies, spawnMaxBounderies
                    )
                );
            }
        }
    }

    void SpawnSoldier() {

    }
}