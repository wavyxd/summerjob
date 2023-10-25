using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{

    public GameObject levelOne;
    public GameObject tutorialLevel;
    public GameObject levelTwo;
    public void CampsiteLevel()
    {
        //SceneManager.LoadScene("MainScene");
        levelOne.SetActive(true);
        
    }

    public void Tutorial()
    {
        //SceneManager.LoadScene("Tutorial");
       tutorialLevel.SetActive(true);
    }

    public void LevelThree()
    {
        levelTwo.SetActive(true);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Menyoo");
    }
}
