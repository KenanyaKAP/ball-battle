using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCanvasUI : MonoBehaviour
{
    public static EndGameCanvasUI instance;

    [Header("UI Component")]
    [SerializeField] GameObject gameplayUI;
    [SerializeField] GameObject player1Win;
    [SerializeField] GameObject player2Win;
    [SerializeField] GameObject draw;
    [SerializeField] GameObject trophy;

    void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }

    public void EndGame(WhichPlayer winner) {
        gameObject.SetActive(true);
        gameplayUI.SetActive(false);

        if (winner == WhichPlayer.Player1) {
            player1Win.SetActive(true);
            player2Win.SetActive(false);
            draw.SetActive(false);
            trophy.SetActive(true);
        } else if (winner == WhichPlayer.Player2) {
            player1Win.SetActive(false);
            player2Win.SetActive(true);
            draw.SetActive(false);
            trophy.SetActive(true);
        } else if (winner == WhichPlayer.None) {
            player1Win.SetActive(false);
            player2Win.SetActive(false);
            draw.SetActive(true);
            trophy.SetActive(false);
        } 
    }
}
