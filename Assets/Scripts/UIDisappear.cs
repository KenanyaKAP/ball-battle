using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisappear : MonoBehaviour
{
    [SerializeField] float lifeTime = 1f;
    [SerializeField] float deltaYLocation = 50f;
    [SerializeField] CanvasGroup canvasGroup;

    void Start() {
        LeanTween.value(1, 0, lifeTime).setEaseOutSine().setOnUpdate((float value) => {
            canvasGroup.alpha = value;
        });
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + deltaYLocation, lifeTime).setEaseOutSine().setOnComplete(() => {
            Destroy(gameObject);
        });
    }
}
