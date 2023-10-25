using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontStopSound : MonoBehaviour
{
   void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
