using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenBar : MonoBehaviour
{
    public Transform loadingBarImage;
    public float TargetAmount = 100.0f;
    public float Speed = 30;
    public string scene;
    private float CurrentAmount = 0;

    void Start()
    {
        CurrentAmount = 10.0f;
    }

    
    void Update()
    {
        CurrentAmount += Speed * Time.deltaTime;
        loadingBarImage.GetComponent<Image>().fillAmount = (float)CurrentAmount / 100.0f;
        CurrentAmount= Mathf.Clamp(CurrentAmount, 0, TargetAmount);
        if (CurrentAmount==TargetAmount)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
