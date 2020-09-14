using System;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialStepWayToPass { CLICK_TO_CONTINUE, CLICK_IN_CLICKABLE_ELEMENT }

[Serializable]
public class TutorialStep
{
    public string name;
    public TutorialStepWayToPass wayToPass;
    public bool startOfStage;
    public bool endOfStage;
    public Text infoText;
    public GameObject clickableElement;
}
