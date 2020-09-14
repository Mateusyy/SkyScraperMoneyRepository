using Firebase.Analytics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialStep> tutorialSteps;
    private Animator anim;
    public GameObject tutorialBackground;

    private bool tutorialIsActive = false;
    private int _index;
    private TutorialStep tutorialStep;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayTutorialStep(int index)
    {
        FirebaseAnalytics.LogEvent("Tutorial", new Parameter("Step_Number", index));

        _index = index;
        tutorialIsActive = true;

        anim.SetTrigger(string.Format("Step_{0}_Show", index));
        tutorialStep = tutorialSteps[index];

        if (tutorialStep.startOfStage)
        {
            tutorialBackground.SetActive(true);
        }
    }

    public void HideTutorialStep()
    {
        tutorialIsActive = false;

        anim.SetTrigger(string.Format("Step_{0}_Hide", _index));

        if(tutorialStep.endOfStage)
        {
            if(tutorialBackground.activeSelf)
                tutorialBackground.SetActive(false);
        }
        else
        {
            PlayTutorialStep(_index + 1);
        }
    }

    public void UpdateTextInfo()
    {
        string nameTagForLocalizationManager = "Tutorial_" + _index;
        tutorialStep.infoText.text = LocalizationManager.instance.StringForKey(nameTagForLocalizationManager);
    }

    private void Update()
    {
        if (tutorialIsActive)
        {
            switch (tutorialStep.wayToPass)
            {
                case TutorialStepWayToPass.CLICK_TO_CONTINUE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        HideTutorialStep();
                    }
                    break;
                case TutorialStepWayToPass.CLICK_IN_CLICKABLE_ELEMENT:
                    if(tutorialStep.clickableElement.GetComponent<Button>() != null)
                    {
                        tutorialStep.clickableElement.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            HideTutorialStep();
                            tutorialStep.clickableElement.GetComponent<Button>().onClick.RemoveListener(() => { });
                        });
                    }
                    break;
                default:
                    break;
            }
        }
    }
}