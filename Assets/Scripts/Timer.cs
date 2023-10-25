using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static float timeRemaining;
    public static bool timerIsRunning = false;
    public Text timeText;
    public static bool GameOver = false;
    public GameObject gameOver;

    private void Start()
    {
        timerIsRunning = true;
        timeRemaining = 120;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining >0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                GameOver = true;
                
            }
        }
        if (GameOver == true)
        {
            gameOver.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
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