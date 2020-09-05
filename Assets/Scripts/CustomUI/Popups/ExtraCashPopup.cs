using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraCashPopup : MonoBehaviour
{
    [SerializeField]
    private Button triggerButton;

    public void WhenAnimationStart_Caller()
    {
        SetButtonActivity(false);
    }

    public void WhenAnimationEnd_Caller()
    {
        SetButtonActivity(true);
    }

    public void SetButtonActivity(bool value)
    {
        if (value == true)
        {
            if (triggerButton.IsActive())
                triggerButton.interactable = true;
        }
        else
        {
            if (triggerButton.IsActive())
                triggerButton.interactable = false;
        }
    }
}
