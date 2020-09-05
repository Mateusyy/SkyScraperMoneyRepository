using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineEarning : MonoBehaviour
{
    public delegate void EventHandler();
    public EventHandler OnPopupClose;

    private Animator anim;

    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text description_one;
    [SerializeField]
    private Text description_two;
    [SerializeField]
    private Text descriptionMoneyText;
    [SerializeField]
    private Text collectText_one;
    [SerializeField]
    private Text collectText_two;

    private float offlineEarning;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Initialize(float offlineEarning)
    {
        this.offlineEarning = offlineEarning;

        /*if(LocalizationManager.instance != null)
        {
            titleText.text = LocalizationManager.instance.StringForKey("offlinePopupTitle");
            description_one.text = LocalizationManager.instance.StringForKey("offlinePopupDescOne");
            descriptionMoneyText.text = NumberFormatter.ToString(number: offlineEarning, showDecimalPlaces: true, showDollarSign: true);
            description_two.text = LocalizationManager.instance.StringForKey("offlinePopupDescTwo");
            collectText_one.text = LocalizationManager.instance.StringForKey("offlinePopupCollectOne");
            collectText_two.text = LocalizationManager.instance.StringForKey("offlinePopupCollectTwo");
        }
        else
        {*/
            titleText.text = "Welcome back!";
            description_one.text = "You earn";
            descriptionMoneyText.text = NumberFormatter.ToString(number: offlineEarning, showDecimalPlaces: true, showDollarSign: true);
            description_two.text = "when you weren't in your business!";
            collectText_one.text = "Collect";
            collectText_two.text = "Watch ad and <color=#22EE11>double</color> your profit!";
        //}
    }

    public void Display()
    {
        SetVisibleInterectable(true);
    }

    public void Hide()
    {
        SetVisibleInterectable(false);
    }

    private void SetVisibleInterectable(bool isVisible)
    {
        /*CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = (isVisible ? 1f : 0f);
        canvasGroup.interactable = isVisible;
        canvasGroup.blocksRaycasts = isVisible;*/

        if (isVisible)
        {
            anim.SetTrigger("Show");
        }
        else
        {
            anim.SetTrigger("Hide");
        }
    }

    public void ExitButtonPressed()
    {
        PlayerManager.instance.IncrementCashBy(offlineEarning);

        Hide();
        if(OnPopupClose != null)
        {
            OnPopupClose();
        }
    }

    public float GetCalculatedOfflineEarningValue()
    {
        return offlineEarning;
    }

    public void ExitAndWatchADButtonPressed()
    {
        Hide();
        GameManager.instance.OnADSDoubleOfflineEarningButtonPressed();

        if (OnPopupClose != null)
        {
            OnPopupClose();
        }
    }
}
