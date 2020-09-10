using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuySlotPanel : MonoBehaviour
{
    public delegate void EventHandler();

    public EventHandler OnBuyBusinessButtonPressed;

    public int index;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text costText;
    [SerializeField]
    private CanvasGroup availableRectangle;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private CanvasGroup timerToUnlockUI;
    [SerializeField]
    private Text timerToUnlock_text;
    [SerializeField]
    private Image progressBarFill;
    [SerializeField]
    private CanvasGroup unlockButton;
    [SerializeField]
    private CanvasGroup adsButton;
    [SerializeField]
    private CanvasGroup finishRepairButton;

    [SerializeField]
    private Text PressToSpeedUpRenovationText;
    [SerializeField]
    private Text FloorRenovationText;

    private string _name;
    public int numberOfBuilding;
    public float timerToUnlock;
    public float cost;

    public bool afterBooster = false;
    private bool shoudAddFunctionForUnlockButton = true;

    private float secondsLeft;

    public bool interactable
    {
        get { return buyButton.interactable; }
        set
        {
            buyButton.interactable = value;
            availableRectangle.alpha =
                (
                    value ? 1.0f : 0.0f
                );
        }
    }

    public bool nowIsUnlocking
    {
        get { return PlayerManager.instance.buyButtonParamsToUnlock[index].unlockingIsActive; }
    }

    public void Initialize(int numberOfBuilding, string name, float cost, float timerToUnlock)
    {
        this.numberOfBuilding = numberOfBuilding;
        this._name = name;
        this.timerToUnlock = timerToUnlock;
        this.cost = cost;
        costText.text = NumberFormatter.ToString(cost, showDecimalPlaces: false);
        buyButton.onClick.AddListener(() =>
        {
            FindObjectOfType<TasksPopup>().Show(this.gameObject.GetComponent<TasksManager>());
            //OnBuyBusinessButtonPressed();
        });

        adsButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            UnityADSManager.instance.indexOfSlotForBenefits = index;
            UnityADSManager.instance.ShowRewardedVideo(UnityADSManager.BoosterType.cutTimerToUnlockSlot);
        });

        finishRepairButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            FindObjectOfType<RepairFinishPopup>().GetComponent<RepairFinishPopup>().ShowPopup(this, secondsLeft);
        });
        RefreshLanguageBuySlotPanel();
    }

    public void ShowTimerToUnlockUI()
    {
        PlayerManager.instance.buyButtonParamsToUnlock[index].unlockSlotStartDateTime = (ulong)DateTime.Now.Ticks;
        PlayerManager.instance.buyButtonParamsToUnlock[index].unlockingIsActive = true;
    }

    public void RefreshLanguageBuySlotPanel()
    {
        string upperName = _name.ToUpper();
        nameText.text = LocalizationManager.instance.StringForKey(upperName);

        PressToSpeedUpRenovationText.text = LocalizationManager.instance.StringForKey("Renovation_pressToSpeedUp");
        FloorRenovationText.text = LocalizationManager.instance.StringForKey("Renovation_floorRenovation");
    }

    private void OnDestroy()
    {
        OnBuyBusinessButtonPressed = null;
    }

    private void SetUpSecondLeft()
    {
        secondsLeft = 0f;
    }

    private void SetUpUnlockingIsActive()
    {
        PlayerManager.instance.buyButtonParamsToUnlock[index].unlockingIsActive = false;
    }

    void Update()
    {
        if (PlayerManager.instance.buyButtonParamsToUnlock[index].unlockingIsActive)
        {
            timerToUnlockUI.alpha = 1f;
            timerToUnlockUI.blocksRaycasts = true;
            timerToUnlockUI.interactable = true;

            ulong diff = ((ulong)DateTime.Now.Ticks - PlayerManager.instance.buyButtonParamsToUnlock[index].unlockSlotStartDateTime);
            ulong m = diff / TimeSpan.TicksPerMillisecond;

            secondsLeft = (float)(timerToUnlock * 1000f - m) / 1000.0f;

            string r = "";
            //H
            r += ((int)secondsLeft / 3600).ToString() + ":";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            //M
            r += ((int)secondsLeft / 60).ToString("00") + ":";
            //S
            r += (secondsLeft % 60).ToString("00") + "";

            timerToUnlock_text.text = r;

            progressBarFill.fillAmount = (timerToUnlock - secondsLeft) / timerToUnlock;

        }

        if((secondsLeft <= 0f && PlayerManager.instance.buyButtonParamsToUnlock[index].unlockingIsActive == true) || 
           afterBooster == true)
        {
            unlockButton.alpha = 1f;
            unlockButton.interactable = true;
            unlockButton.blocksRaycasts = true;

            if (shoudAddFunctionForUnlockButton)
            {
                unlockButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (afterBooster)
                        afterBooster = !afterBooster;

                    SetUpSecondLeft();
                    SetUpUnlockingIsActive();
                    GameManager.instance.OnBuySlotButtonUnlockPressed(index);
                });

                shoudAddFunctionForUnlockButton = false;
            }
        }
    }
}

[Serializable]
public struct BuyButtonParamsToUnlock
{
    [SerializeField]
    public ulong unlockSlotStartDateTime;
    [SerializeField]
    public bool unlockingIsActive;
}
