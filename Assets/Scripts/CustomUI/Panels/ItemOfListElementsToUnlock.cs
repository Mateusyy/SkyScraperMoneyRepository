using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemOfListElementsToUnlock : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private CanvasGroup lockedPanel;
    [SerializeField]
    private CanvasGroup availablePanel;

    [SerializeField]
    private Sprite lockedPanelSprite;
    [SerializeField]
    private Sprite availablePanelSprite;

    [SerializeField]
    private Text lockedPanel_LevelText;
    [SerializeField]
    private Text lockedPanel_ToUnlockText;

    [SerializeField]
    private Text availablePanel_NameText;
    [SerializeField]
    private Image availablePanel_IconOfProfit;
    [SerializeField]
    private Text availablePanel_ValueText;

    [SerializeField]
    private Button mainButton;

    private Slot slot;
    public InteriorElement interiorElement;
    private int index;


    private void Start()
    {
    }

    public void Initialize(Slot slot, InteriorElement interiorElement, int index)
    {
        anim = GetComponent<Animator>();

        this.slot = slot;
        this.interiorElement = interiorElement;
        this.index = index;

        mainButton.onClick.AddListener(() => 
        {
            OnMainButtonPressed(interiorElement, index);
        });

        switch (interiorElement.status)
        {
            case InteriorElementStatus.NOT_AVAILABLE:
                TurnOffCanvasGroup(availablePanel);
                TurnOnCanvasGroup(lockedPanel);

                SetLockedPanelProperty(index);
                SetMainButtonNotInterectable();
                break;
            case InteriorElementStatus.AVAILABLE:
                TurnOffCanvasGroup(availablePanel);
                TurnOnCanvasGroup(lockedPanel);

                SetLockedPanelProperty(index);
                SetMainButtonInterectable();
                break;
            case InteriorElementStatus.BOUGHT:
                TurnOffCanvasGroup(lockedPanel);
                TurnOnCanvasGroup(availablePanel);

                SetAvailablePanelProperty(interiorElement, index);
                break;
            default:
                break;
        }
    }

    private void OnMainButtonPressed(InteriorElement interiorElement, int index)
    {
        float value = slot.GetMilestoneValue(index);
        
        if(interiorElement.symbol == null)
            FindObjectOfType<BigUpgradeFloorsForAnimationPopup>().Show(interiorElement.name, value.ToString() + "x", interiorElement.GetComponent<SpriteRenderer>().sprite);
        else
            FindObjectOfType<BigUpgradeFloorsForAnimationPopup>().Show(interiorElement.name, value.ToString() + "x", interiorElement.symbol);

        PlayerManager.instance.DecrementCashBy(interiorElement.price);
        interiorElement.status = InteriorElementStatus.BOUGHT;
        slot.SetUnlockedObject(index);
        interiorElement.TurnOnElement();

        SetAvailablePanelProperty(interiorElement, index);
        TurnOffCanvasGroup(lockedPanel);
        TurnOnCanvasGroup(availablePanel);

        slot.UpdateLevelBy(0);  //only to get multiplier
        GameManager.instance.OnUpdateUI();
        mainButton.interactable = false;
    }

    private void SetLockedPanelProperty(int index)
    {
        gameObject.GetComponent<Image>().sprite = lockedPanelSprite;

        lockedPanel_LevelText.text = slot.GetMilestoneLevelTarget(index).ToString() + " lvl";
        lockedPanel_ToUnlockText.text = "to unlock";
    }

    private void SetLockedPanelPropertyWhenLevelWasReached(InteriorElement interiorElement)
    {
        lockedPanel_LevelText.text = NumberFormatter.ToString(interiorElement.price, false, true, false);
        lockedPanel_ToUnlockText.text = "to buy";

        if(PlayerManager.instance.cash >= interiorElement.price)
        {
            SetMainButtonInterectable();
        }
        else
        {
            SetMainButtonNotInterectable();
        }
    }

    private void SetMainButtonInterectable()
    {
        mainButton.interactable = true;
        anim.SetTrigger("PlayAvailableAnim");
    }

    private void SetMainButtonNotInterectable()
    {
        mainButton.interactable = false;
        anim.SetTrigger("StopAvailableAnim");
    }

    private void SetAvailablePanelProperty(InteriorElement interiorElement, int index)
    {
        gameObject.GetComponent<Image>().sprite = availablePanelSprite;

        availablePanel_NameText.text = interiorElement.name;
        if(interiorElement.symbol == null)
            availablePanel_IconOfProfit.sprite = interiorElement.GetComponent<SpriteRenderer>().sprite;
        else
            availablePanel_IconOfProfit.sprite = interiorElement.symbol;

        float value = slot.GetMilestoneValue(index);
        
        availablePanel_ValueText.text = value.ToString() + "x";
    }

    private void TurnOnCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void TurnOffCanvasGroup(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void UpdateStatusOfButton(int level)
    {
        if(level >= slot.GetMilestoneLevelTarget(index))
        {
            SetLockedPanelPropertyWhenLevelWasReached(interiorElement);
        }
    }
}
