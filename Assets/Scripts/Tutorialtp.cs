using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorialtp : MonoBehaviour
{
   void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("LevelSelect");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
