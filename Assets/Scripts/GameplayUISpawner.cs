using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUISpawner : MonoBehaviour
{
    public static GameplayUISpawner instance;
    
    [SerializeField] Transform parent;
    
    [Header("UI Assets")]
    [SerializeField] GameObject notEnoughEnergyTextBlue;
    [SerializeField] GameObject notEnoughEnergyTextRed;

    [Header("Match Pop Up")]
    [SerializeField] GameObject player1PopUp;
    [SerializeField] GameObject player2PopUp;
    [SerializeField] GameObject drawPopUp;

    void Awake() {
        instance = this;
    }

    public void SpawnNotEnoughEnergyText(bool isBlue, Vector3 worldPos) {
        GameObject go = Instantiate(isBlue ? notEnoughEnergyTextBlue : notEnoughEnergyTextRed, worldPos - Vector3.back * 5f, parent.rotation, parent);
    }

    public void PopUpMatch(WhichPlayer winner) {
        if (winner == WhichPlayer.Player1) {
            player1PopUp.SetActive(true);
        } else if (winner == WhichPlayer.Player2) {
            player2PopUp.SetActive(true);
        } else if (winner == WhichPlayer.None) {
            drawPopUp.SetActive(true);
        } 
    }

    public void EndGame() {
        
    }
}
