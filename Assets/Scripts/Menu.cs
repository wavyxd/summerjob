using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string firstLevel;
    public GameObject optionsScreen;
    
    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
        
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void Select()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ControlLoad()
    {
        SceneManager.LoadScene("Controls");
    }
}
