using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    float currentTime = 0f;
    float startingTime = 4f;
    [SerializeField] Text countdownText;
    [SerializeField] GameObject player;
    [SerializeField] GameObject countdown;
    [SerializeField] GameObject prestartCamera;
    [SerializeField] GameObject timer;

    void Start()
    {
        currentTime = startingTime;
        player.SetActive(false);
        timer.SetActive(false);
    }

    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0)
        {
            //currentTime = 0;
            countdown.SetActive(false);
            player.SetActive(true);
            prestartCamera.SetActive(false);
            timer.SetActive(true);
        }
    }
}
