using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Bar : MonoBehaviour
{
    public static float amount;
    private float lerpTimer;
    public float maxAmount = 100f;
    public float chipSpeed = 3f;
    public Image front;
    public Image back;
    public GameObject gameDone;
    public AudioSource win;
    public float collectAmount;
    void Start()
    {
        amount = 20;
    }


    void Update()
    {
        amount = Mathf.Clamp(amount, 0, maxAmount);
        UpdateBar();
        
        if (amount == 100)
        {
            gameDone.SetActive(true);

            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
           
        }
    }

    public void UpdateBar()
    {
        float fillF = front.fillAmount;
        float fillB = back.fillAmount;
        float hFraction = amount / maxAmount;
        if (fillF < hFraction)
        {
            back.color = Color.green;
            back.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            front.fillAmount = Mathf.Lerp(fillF, back.fillAmount, percentComplete);
        }
    }

    public void FillBar(float _amount)
    {
        amount += _amount;
        lerpTimer = 0f;
    }

    public void OnTriggerEnter(Collider other)
    {
       
        FillBar(collectAmount);
    }


}
