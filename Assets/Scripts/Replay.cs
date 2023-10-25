using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public static bool restart;
    public AudioSource win;

    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Win()
    {
        if (Bar.amount == 100)
        {
            win.Play();
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Timer.GameOver = false;
        this.gameObject.SetActive(true);
    }

    public void LevelSelect()
    {
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("LevelSelect");
    }
}