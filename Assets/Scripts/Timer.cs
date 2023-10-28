using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public Text timeText;
    // public TextMeshProUGUI ScoreTextkiri;
    // public TextMeshProUGUI ScoreTextkanan;
   // public GameObject uiObject;
    // public GameObject uiObject2;
    // public CollisionScore score;
    // public CollisionScore score1;
    // public AudioSource y;
    // public AudioSource s;

    private void Start()
    {
        timerIsRunning = true;
        //uiObject.SetActive(false);
        //uiObject2.SetActive(false);

        // score = GameObject.Find("Cube").GetComponent<CollisionScore>();
        // score1 = GameObject.Find("Cube(1)").GetComponent<CollisionScore>();
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
               // s.Stop();
                Time.timeScale = 0f;
                //SceneManager.LoadScene("FalseScene");
                
                // if(score._score > score1._score){
                //     uiObject.SetActive(true);                    
                //     y.Play();
                // }

                // else if(score1._score > score._score){
                //     uiObject2.SetActive(true);
                //     y.Play();
                // }
                //if score p1>p2; set active p1 win
                //else if score p2>p1; set active p2 win
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}