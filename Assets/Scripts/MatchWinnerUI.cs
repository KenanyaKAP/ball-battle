using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchWinnerUI : MonoBehaviour
{
    [SerializeField] Vector3 startLocalPos;
    
    void Awake() {
        startLocalPos = transform.localPosition;
        gameObject.SetActive(false);
    }

    void OnEnable() {
        PopUp();
    }

    void PopUp() {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, .3f).setEaseOutQuad();
        LeanTween.value(gameObject, 0, 1, 2.8f).setOnComplete(() => {
            LeanTween.scale(gameObject, Vector3.zero, .3f).setEaseOutQuad().setOnComplete(() => {
                gameObject.SetActive(false);
            });
        });
    }
}
