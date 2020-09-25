using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum TypeOfUpgradeSystem { ADVANCED, SIMPLE };


public class UpgradeEachFloorPopup : MonoBehaviour
{
    public TypeOfUpgradeSystem typeOfUpgradeSystem;

    public List<UpgradeEachFloor_Button> buttons = new List<UpgradeEachFloor_Button>();

    public Animator anim;
    private Slot slot;

    [SerializeField]
    private Button bulkButton;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text downPanelTitle;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text levelValue;
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text moneyValue;
    [SerializeField]
    private Text profitText;
    [SerializeField]
    private Text profitValue;
    [SerializeField]
    private Text costText;
    [SerializeField]
    private Text costValue;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private Text upgradeButton_Text;
    [SerializeField]
    private ParticleSystem upgreadeButtonParticleSystem;

    [SerializeField]
    private RectTransform scrollViewContent;
    [SerializeField]
    private ItemOfListElementsToUnlock itemOfListElementsToUnlockPrefab;

    public bool isVisible = false;
    public List<ItemOfListElementsToUnlock> itemsOfListElementsToUnlock = new List<ItemOfListElementsToUnlock>();

    private void Awake()
    {
        if(typeOfUpgradeSystem == TypeOfUpgradeSystem.ADVANCED)
        {
            if(bulkButton != null) bulkButton.gameObject.SetActive(false);
        }
        else
        {
            if (bulkButton != null) bulkButton.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(Slot slot, InteriorPanel interior)
    {
        if(PlayerPrefs.GetInt("TutorialUpgradeEachFloor") != 1)
        {
            FindObjectOfType<TutorialManager>().PlayTutorialStep(2);
            PlayerPrefs.SetInt("TutorialUpgradeEachFloor", 1);
        }

        this.slot = slot;

        SetUpButton();
        SetUpButtons(slot.bulkLevelUpIndex);
        SetUpTexts(slot.bulkLevelUpIndex);

        GenerateItemsToUnlock(slot, interior);
        
        anim.SetTrigger("Show");
        isVisible = true;
    }

    private void GenerateItemsToUnlock(Slot slot, InteriorPanel interior)
    {
        for (int i = 0; i < interior.interiorObjects.Count; i++)
        {
            ItemOfListElementsToUnlock item = Instantiate(itemOfListElementsToUnlockPrefab, scrollViewContent);
            item.Initialize(slot, interior.interiorObjects[i], i);
            itemsOfListElementsToUnlock.Add(item);
        }
    }

    public void CloseButton_OnPressed()
    {
        Hide();
    }

    public void Func_ClearListOfObjectsToUnlock()
    {
        for (int i = 0; i < scrollViewContent.childCount; i++)
        {
            Destroy(scrollViewContent.GetChild(i).gameObject);
        }

        itemsOfListElementsToUnlock.Clear();
    }

    private void Hide()
    {
        anim.SetTrigger("Hide");
        isVisible = false;
    }

    private void SetUpButtons(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(i == index)
            {
                buttons[i].SetAvailable();
            }
            else
            {
                buttons[i].SetDisable();
            }
        }
    }

    private void SetUpTexts(int index)
    {
        title.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_Title");
        downPanelTitle.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_DownPanelTitle");
        upgradeButton_Text.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_UpgradeButtonText");

        levelText.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_Level");
        moneyText.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_Money");
        profitText.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_Profit");
        costText.text = LocalizationManager.instance.StringForKey("UpgradeEachFloor_Cost");

        if(index >= 3 && slot.DetermineMaximumNumberOfLevelsPlayerCanUpgrade() <= 0f)
        {
            levelValue.text = slot.level + "+" + slot.DetermineMaximumNumberOfLevelsPlayerCanUpgrade();
            moneyValue.text = "-";
            profitValue.text = "-";
            costValue.text = "-";
            return;
        }

        levelValue.text = index < 3 
            ? 
            slot.level + "<color=#8FFF64>" + "+" + Constant.BULK_UPGRADE_LEVELS[index] + "</color>" : 
            slot.level + "<color=#8FFF64>" + "+" + slot.DetermineMaximumNumberOfLevelsPlayerCanUpgrade() + "</color>";

        moneyValue.text = index < 3
            ?
            moneyValue.text = "<color=#8FFF64>" + "+" + NumberFormatter.ToString(slot.CashPerSecondForLevel(Constant.BULK_UPGRADE_LEVELS[index]), true, true) + "/s" + "</color>" :
            moneyValue.text = "<color=#8FFF64>" + "+" + NumberFormatter.ToString(slot.CashPerSecondForLevel(slot.DetermineMaximumNumberOfLevelsPlayerCanUpgrade()), true, true) + "/s" + "</color>";

        profitValue.text = index < 3
            ?
            LocalizationManager.instance.StringForKey("UpgradeEachFloor_Speed") + "<color=#8FFF64>" + " +" + Math.Round(slot.timeSpeedAfterEachUpgrade * Constant.BULK_UPGRADE_LEVELS[index], 2) + "s" + "</color>":
            LocalizationManager.instance.StringForKey("UpgradeEachFloor_Speed") + "<color=#8FFF64>" + " +" + Math.Round(slot.timeSpeedAfterEachUpgrade * slot.DetermineMaximumNumberOfLevelsPlayerCanUpgrade(), 2) + "s" + "</color>";
        costValue.text = index < 3
            ?
            NumberFormatter.ToString(slot.UpgreadeXLevelsCost(Constant.BULK_UPGRADE_LEVELS[index]), true) :
            NumberFormatter.ToString(slot.UpgreadeMaxLevelsCost(), true);
    }

    private void SetUpButton()
    {
        if(typeOfUpgradeSystem == TypeOfUpgradeSystem.ADVANCED)
        {
            upgradeButton.interactable = PlayerManager.instance.cash >= slot.currentCostValue ? true : false;
        }
        else
        {
            //upgradeButton.interactable = PlayerManager.instance.cash >= slot.currentCostValue ? true : false;
        }
    }

    public void PanelButton_OnPressed(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(i == index)
            {
                buttons[i].SetAvailable();
                slot.bulkLevelUpIndex = index;
                SetUpTexts(slot.bulkLevelUpIndex);
                slot.currentCostValue = slot.CalculateCurrentCostValueAdvanced();
                SetUpButton();
            }
            else
            {
                buttons[i].SetDisable();
            }
        }
    }

    public void SetSlot(Slot slot)
    {
        this.slot = slot;
    }

    public void UpgradeButton_OnPressed()
    {
        bool playParticle = slot.UpgreadeToLastCalculateLevel();
        if(typeOfUpgradeSystem == TypeOfUpgradeSystem.ADVANCED)
        {
            slot.currentCostValue = slot.CalculateCurrentCostValueAdvanced();

            if (playParticle)
            {
                PlayUpgreadeParticle();
            }
        }
        else //SIMPLE
        {
            slot.currentCostValue = slot.CalculateCurrentCostValueSimple();
        }
        SetUpButton();
        PlayUpgradeSound();

        GameManager.instance.panels[slot.index].GetComponent<SlotPanel>().Refresh();
    }

    public void Refresh()
    {
        SetUpTexts(slot.bulkLevelUpIndex);
        SetUpButton();
        RefreshStatusOfButtonsInObjectsToUnlock();
    }

    private void RefreshStatusOfButtonsInObjectsToUnlock()
    {
        for (int i = 0; i < itemsOfListElementsToUnlock.Count; i++)
        {
            itemsOfListElementsToUnlock[i].UpdateStatusOfButton(slot.level);
        }
    }

    private void PlayUpgreadeParticle()
    {
        upgreadeButtonParticleSystem.Play();
    }

    private void PlayUpgradeSound()
    {
        AudioSource upgradeAudioSource = upgradeButton.GetComponent<AudioSource>();
        if (SettingsGame.instance.isSound)
        {
            upgradeAudioSource.Play();
        }
    }
}

[Serializable]
public struct UpgradeEachFloor_Button
{
    [SerializeField]
    private Button buttonGO;
    [SerializeField]
    private Sprite disableSprite;
    [SerializeField]
    private Sprite availableSprite;

    public void SetAvailable()
    {
        buttonGO.image.sprite = availableSprite;
    }

    public void SetDisable()
    {
        buttonGO.image.sprite = disableSprite;
    }
}
