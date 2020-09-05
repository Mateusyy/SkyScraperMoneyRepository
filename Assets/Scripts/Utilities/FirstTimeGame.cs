using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeGame : MonoBehaviour
{
    public void ActionAfterFirstTimeAnimation()
    {
        GameManager.instance.InitAfterFirstTimeAnimation();
        GameManager.instance.tutorialPopup.SetActive(true);
        FindObjectOfType<TutorialPopup>().StartFirstTutorial();
    }
}
