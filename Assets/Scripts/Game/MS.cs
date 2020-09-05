using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MS : MonoBehaviour
{
    public static MS instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as MS;
            DontDestroyOnLoad(gameObject);    
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }
}
