using UnityEngine;

public class ColDet3 : MonoBehaviour
{
    public AudioSource collection;
    public void OnTriggerEnter(Collider other)
    {
       
            this.gameObject.SetActive(false);
        collection.Play();
        
    }
}