using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    [SerializeField]
    private Text text_small;
    [SerializeField]
    private Text text_big;

    public int targetSteps = 7;
    private int currentSteps = 0;

    public Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        currentSteps = 0;
        targetSteps = 7;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentSteps += 1;

            if (currentSteps > 0)
            {
                anim.SetTrigger("NextAnim");
            }
        }

        if(currentSteps >= targetSteps)
        {
            //end tutorial
            gameObject.SetActive(false);
            FirebaseInit.RegisterEvent("FinishTutorial", "Value", 1);
        }
    }

    public void StartFirstTutorial()
    {
        anim.SetTrigger("PlayFirstTutorial");
        FirebaseInit.RegisterEvent("StartTutorial", "Value", 1);
    }

    public void UpdateTextWithBigPanel()
    {
        string nameTagForLocalizationManager = "Tutorial_" + currentSteps;
        text_big.text = LocalizationManager.instance.StringForKey(nameTagForLocalizationManager);
    }

    public void UpdateTextWithSmallPanel()
    {
        string nameTagForLocalizationManager = "Tutorial_" + currentSteps;
        text_small.text = LocalizationManager.instance.StringForKey(nameTagForLocalizationManager);
    }
}
