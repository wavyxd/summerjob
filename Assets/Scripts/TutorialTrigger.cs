using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject firstPrompt;
    public GameObject secondPrompt;
    public GameObject bar;


    public void Start()
    {
        
        bar.SetActive(false);
    }
    void OnTriggerEnter(Collider collider)
    {
        firstPrompt.SetActive(false);
        secondPrompt.SetActive(true);
        bar.SetActive(true);
        this.gameObject.SetActive(false);
        Cursor.visible = false;
    }
}
