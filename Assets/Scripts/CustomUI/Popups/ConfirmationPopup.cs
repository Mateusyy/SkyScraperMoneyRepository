using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConfirmationPopupStatus { YES, NO, NONE };

public class ConfirmationPopup : MonoBehaviour
{
    [Header("Small Panel Prop")]
    [SerializeField]
    private CanvasGroup smallPanel;
    [SerializeField]
    private Text smallTitle;
    [SerializeField]
    private Text smallText;
    [SerializeField]
    private Text small_noButton_Text;
    [SerializeField]
    private Text small_yesButton_Text;

    [Header("Big Panel Prop")]
    [SerializeField]
    private CanvasGroup bigPanel;
    [SerializeField]
    private Text bigTitle;
    [SerializeField]
    private Text bigText_1;
    [SerializeField]
    private Text bigText_2;
    [SerializeField]
    private Text big_noButton_Text;
    [SerializeField]
    private Text big_yesButton_Text;

    private ConfirmationPopupStatus result = ConfirmationPopupStatus.NONE;
    private Animator anim;

    public delegate void DelegateAfterConfirmation();
    public static DelegateAfterConfirmation afterConfirmationDelegate;

    private string XP = string.Empty;
    private float xpValue;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator CallConfirmationPanel(string textTag, int indexOfPanel = 0, float xp = 0)
    {
        if(xp > 0)
        {
            this.XP = NumberFormatter.ToString(xp, false, false);
            this.xpValue = xp;
        }

        result = ConfirmationPopupStatus.NONE;

        TurnCorrectPanel(indexOfPanel);
        SetText(textTag, indexOfPanel);

        small_noButton_Text.text = LocalizationManager.instance.StringForKey("NoButtonText");
        small_yesButton_Text.text = LocalizationManager.instance.StringForKey("YesButtonText");
        big_noButton_Text.text = LocalizationManager.instance.StringForKey("NoButtonText");
        big_yesButton_Text.text = LocalizationManager.instance.StringForKey("YesButtonText");

        Show();

        yield return StartCoroutine(ConfirmationLogic());

        Hide();

        if (result == ConfirmationPopupStatus.YES)
            afterConfirmationDelegate();
    }

    private void TurnCorrectPanel(int index)
    {
        switch (index)
        {
            case 0:
                smallPanel.alpha = 1;
                smallPanel.blocksRaycasts = true;
                smallPanel.interactable = true;
                bigPanel.alpha = 0;
                bigPanel.blocksRaycasts = false;
                bigPanel.interactable = false;
                break;
            case 1:
                bigPanel.alpha = 1;
                bigPanel.blocksRaycasts = true;
                bigPanel.interactable = true;
                smallPanel.alpha = 0;
                smallPanel.blocksRaycasts = false;
                smallPanel.interactable = false;
                break;
        }
    }

    private void SetText(string textTag, int indexOfPanel)
    {
        switch (indexOfPanel)
        {
            case 0:
                smallTitle.text = LocalizationManager.instance.StringForKey("ConfirmationPanel_Title");
                smallText.text = LocalizationManager.instance.StringForKey(textTag);
                break;
            case 1:
                bigTitle.text = LocalizationManager.instance.StringForKey("ConfirmationPanel_Title");
                bigText_1.text = LocalizationManager.instance.StringForKey(textTag);
                bigText_2.text = string.Format("+{0}xp", XP);
                break;
        }
        
    }

    IEnumerator ConfirmationLogic()
    {
        while (result == ConfirmationPopupStatus.NONE)
        {
            yield return null;  //wait
        }

        if (result == ConfirmationPopupStatus.YES)
        {
            if (!string.IsNullOrEmpty(XP))
            {
                //StartCoroutine(DatabaseInit.instance.PutAndUpdateScore(Convert.ToInt32(xpValue)));
                XP = string.Empty;
            }
            yield return true;
        }
        else if(result == ConfirmationPopupStatus.NO)
        {
            yield return false;
        }
    }

    /*private IEnumerator AddXPToDatabase(int xp)
    {
        var updateLeaderboardTask = DatabaseInit.instance.UpdateLeaderboard(xp);
        yield return new WaitUntil(() => updateLeaderboardTask.IsCompleted);
    }*/

    private bool GetResult()
    {
        if(result == ConfirmationPopupStatus.NO)
        {
            return false;
        }
        else if(result == ConfirmationPopupStatus.YES)
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
        result = ConfirmationPopupStatus.NO;
    }

    public void YesButtonOnPressed()
    {
        result = ConfirmationPopupStatus.YES;
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
