using UnityEngine;

public class ColDet2 : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
            gameObject.SetActive(false);
    }
}