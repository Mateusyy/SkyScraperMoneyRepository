using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour
{
    [SerializeField]
    private Button iconImage;
    [SerializeField]
    private List<GameObject> interiors;
    [SerializeField]
    private SpriteRenderer floorImage;
    [SerializeField]
    private Image levelFill;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Image progressBarFill;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text profitText;
    [SerializeField]
    private Button upgreadeButton;
    [SerializeField]
    private Text upgreadeCostText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text counter;
    [SerializeField]
    private CanvasGroup adBoosterImage;
    [SerializeField]
    private CanvasGroup tapToSpeedUpText;
    [SerializeField]
    private SpriteRenderer tapToSpeedUpDialog;
    [SerializeField]
    private Animator tapToGainMoney;
    [SerializeField]
    private ParticleSystem speedUpEffect;
    [SerializeField]
    private SpriteRenderer managerIcon;
    [SerializeField]
    private CanvasGroup upgradeArrow;
    
    public Slot slot;

    public GameObject slotImageBackgroundGO;
    public int numberOfBuilding;
    public int slotLevel;
    public int index;
    public InteriorPanel interior = null;

    public void Initialize(Slot slot)
    {
        this.slot = slot;

        try
        {
            if(interiors.Count <= 0)
            {
                throw new Exception("Interiors is equal null");
            }

            if (slot.index <= interiors.Count - 1)
            {
                interior = Instantiate(interiors[slot.index], this.transform).GetComponent<InteriorPanel>();
                interior.Initialize(slot);
            }
            else
            {
                interior = Instantiate(interiors[slot.index % 10], this.transform).GetComponent<InteriorPanel>();
                interior.Initialize(slot);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
        ManagerData managerData = GameData.instance.GetDataForManager(slot.index);
        numberOfBuilding = slot.numberOfBuilding;

        managerIcon.sprite = managerData.imageForSlot;
        managerIcon.gameObject.SetActive(false);

        floorImage.color = slot.floorColor;

        if (PlayerManager.instance.HasBoughtManager(slot.index))
        {
            SetVisibleManagerIcon();

            slot.ManagerStartsWorking();
        }
        else
        {
            slot.isProducing = false;
            slot.OnUnitProduced += SlotProducedAUnit;
            slot.OnUnitProduced += SlotHiredAManager;
        }

        iconImage.onClick.AddListener(() =>
        {
            slot.StartProducingOrSpeedUp();
        });

        upgreadeButton.onClick.AddListener(() =>
        {
            //Test
            FindObjectOfType<UpgradeEachFloorPopup>().Show(slot, interior);
        });

        slotImageBackgroundGO.GetComponent<Image>().sprite = GameManager.instance.slotPanelSprites[numberOfBuilding - 1];

        RefreshLanguage();
        Refresh();
        GameManager.instance.OnUpdateUI();
    }

    public void SetVisibleManagerIcon()
    {
        managerIcon.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (slot.hasManager == false)
        {
            slot.OnUnitProduced -= SlotProducedAUnit;
            slot.OnUnitProduced -= SlotHiredAManager;
        }
    }

    public void Refresh()
    {
        slotLevel = slot.level;

        levelFill.fillAmount = slot.nextMilestonePercentage;
        levelText.text = slot.level.ToString();

        if (slot.upgreadeLevelExists)
        {

            float costToUpgreade;
            if (slot.bulkLevelUpIndex < 3)
            {
                costToUpgreade = slot.UpgreadeXLevelsCost(Constant.BULK_UPGRADE_LEVELS[slot.bulkLevelUpIndex]);
            }
            else
            {
                costToUpgreade = slot.UpgreadeMaxLevelsCost();
            }

            bool canUpgreade = false;
            for (int i = 0; i < interior.interiorObjects.Count; i++)
            {
                if(interior.interiorObjects[i].GetComponent<InteriorElement>().status != InteriorElementStatus.BOUGHT &&
                    slot.level > slot.GetMilestoneLevelTarget(i) &&
                    PlayerManager.instance.cash >= interior.interiorObjects[i].GetComponent<InteriorElement>().price)
                {
                    canUpgreade = true;
                }
            }

            if(canUpgreade != true)
                canUpgreade = PlayerManager.instance.cash >= costToUpgreade;

            if(canUpgreade) //available
            {
                upgradeArrow.alpha = 1.0f;
            }
            else //disable
            {
                upgradeArrow.alpha = 0.0f;
            }

            //upgreadeCostText.text = NumberFormatter.ToString(costToUpgreade, showDecimalPlaces: true);
            upgreadeCostText.text = slot.level.ToString();
        }
        else
        {
            //disable
            upgradeArrow.alpha = 0.0f;
            upgreadeCostText.text = "---";
            Debug.Log("Fix cos tu ma byc!");
        }

        if (slot.shouldShowCashPerSecond)
        {
            if (slot.timeToProduce >= 0.16f)
            {
                progressBarFill.fillAmount = slot.unitCompletePercentage;
            }
            else
            {
                //Debug.Log("NO FILL!");
                //progressBarFill.fillAmount = 1; //no fill
            }

            profitText.text = string.Format("${0}/{1}", NumberFormatter.ToString(number: slot.profit * 1 / slot.timeToProduce, showDecimalPlaces: true, showDollarSign: false), "s");
        }
        else
        {
            if (PlayerManager.instance.HasBoughtManager(slot.index))
            {
                if (slot.timeToProduce >= 0.25f)
                {
                    progressBarFill.fillAmount = slot.unitCompletePercentage;
                    profitText.text = NumberFormatter.ToString(number: slot.profit, showDecimalPlaces: true);
                }
                else
                {
                    profitText.text = NumberFormatter.ToString(number: slot.cashPerSecond, showDecimalPlaces: false, true, false) + " /s";
                    progressBarFill.fillAmount = 1; //no fill
                }
            }
            else
            {
                progressBarFill.fillAmount = slot.unitCompletePercentage;
                profitText.text = NumberFormatter.ToString(number: slot.profit, showDecimalPlaces: true);
            }
        }

        //counter
        counter.text = "Counter: " + PlayerManager.instance.GetValueSlotsCounter(slot.index);

        //timer
        timeText.text = slot.timeDisplayString;
    }

    public void RefreshLanguage()
    {
        nameText.text = LocalizationManager.instance.StringForKey(slot.name.ToUpper());
        tapToSpeedUpText.GetComponent<Text>().text = LocalizationManager.instance.StringForKey("Slot_TapToSpeedUp");
    }

    public void SlotIsBoosting(bool status)
    {
        if (status)
        {
            adBoosterImage.alpha = 1f;
        }
        else
        {
            adBoosterImage.alpha = 0f;
        }
    }

    public void TapToSpeedUpShow(bool status)
    {
        if (status)
        {
            tapToSpeedUpText.alpha = 1f;
        }
        else
        {
            tapToSpeedUpText.alpha = 0f;
        }
    }

    public void TapToSpeedUpDialogShow(bool status)
    {
        Color dialogColor = new Color(255, 255, 255);

        if (status)
        {
            dialogColor.a = 1f;
        }
        else
        {
            dialogColor.a = 0f;
        }

        tapToSpeedUpDialog.color = dialogColor;
    }

    public void TapToGainMoney(bool status)
    {
        if (status)
        {
            tapToGainMoney.SetBool("isShowing", true);
        }
        else
        {
            tapToGainMoney.SetBool("isShowing", false);
        }
    }

    private void SlotProducedAUnit()
    {
        Assert.IsFalse(slot.hasManager);
        //iconImage.interactable = true;
    }

    public void TurnSpeedUpEffect()
    {
        if (slot.valueOfSpeedUp > 0)
        {
            speedUpEffect.Play();
        }
    }

    public void TurnSpeedUpSound()
    {
        if (SettingsGame.instance.isSound)
        {
            AudioSource speedUpEffectSound = speedUpEffect.gameObject.GetComponent<AudioSource>();
            speedUpEffectSound.Play();
        }
    }

    private void SlotHiredAManager()
    {
        if (PlayerManager.instance.HasBoughtManager(slot.index))
        {
            slot.OnUnitProduced -= SlotProducedAUnit;
            slot.OnUnitProduced -= SlotHiredAManager;
        }
    }
}
