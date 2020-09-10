using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RandomBonusPopupStatus { YES, NO, NONE };

public class RandomBonusPopup : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text noButton_Text;
    [SerializeField]
    private Text yesButton_Text;

    private RandomBonusPopupStatus result = RandomBonusPopupStatus.NONE;
    private Animator anim;

    public delegate void DelegateAfterConfirmation();
    public static DelegateAfterConfirmation afterConfirmationDelegate;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator CallConfirmationPanel(string textTag, double value)
    {
        result = RandomBonusPopupStatus.NONE;

        //SetText
        SetText(textTag, value);

        Show();

        yield return StartCoroutine(ConfirmationLogic());

        Hide();

        if (result == RandomBonusPopupStatus.YES)
            afterConfirmationDelegate();
    }

    private void SetText(string textTag, double value)
    {
        text.text = LocalizationManager.instance.StringForKey(textTag) + "<color=#22EE11>" + NumberFormatter.ToString(value, true, true) + "</color>?";

        noButton_Text.text = LocalizationManager.instance.StringForKey("NoButtonText");
        yesButton_Text.text = LocalizationManager.instance.StringForKey("YesButtonText");
    }

    IEnumerator ConfirmationLogic()
    {
        while (result == RandomBonusPopupStatus.NONE)
        {
            yield return null;  //wait
        }

        if (result == RandomBonusPopupStatus.YES)
        {
            yield return true;
        }
        else if (result == RandomBonusPopupStatus.NO)
        {
            yield return false;
        }
    }

    private bool GetResult()
    {
        if (result == RandomBonusPopupStatus.NO)
        {
            return false;
        }
        else if (result == RandomBonusPopupStatus.YES)
        {
            return true;
        }
        else
        {
            throw new Exception("Unavailable value to renturn!");
        }
    }

    public void NoButtonOnPressed()
    {
        result = RandomBonusPopupStatus.NO;
    }

    public void YesButtonOnPressed()
    {
        result = RandomBonusPopupStatus.YES;
    }

    private void Show()
    {
        anim.SetTrigger("Show");
    }

    private void Hide()
    {
        anim.SetTrigger("Hide");
    }
}
