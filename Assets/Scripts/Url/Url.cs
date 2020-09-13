using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Url : MonoBehaviour
{
    public string urlAddress;

    public void OpenUrl()
    {
        Application.OpenURL(urlAddress);
    }
}
