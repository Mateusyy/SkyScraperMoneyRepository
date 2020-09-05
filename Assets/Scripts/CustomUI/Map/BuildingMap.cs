using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMap : MonoBehaviour
{
    [SerializeField]
    private int index;
    [SerializeField]
    private String nameTag;
    [SerializeField]
    private Text buildingAvailableTitle;
    [SerializeField]
    private Text buildingDisableTitle;
    [SerializeField]
    private Text buildingNumberOfAvailableFloors;
    [SerializeField]
    private Text buildingCashPerSeconds;
    [SerializeField]
    private Text buildingCostText;

    [SerializeField]
    private CanvasGroup activeInfoPanel;
    [SerializeField]
    private CanvasGroup toBuyInfoPanel;
    [SerializeField]
    private Sprite disableBuilding;
    [SerializeField]
    private Sprite availableBuilding;

    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Sprite disableBuyButton;
    [SerializeField]
    private Sprite availableBuyButton;

    private BuildingMapData data;

    public void Initialize(int index)
    {
        data = GameData.instance.GetDataForBuildingMap(index);
    }

    public void RefreshUI()
    {
        //available
        if (PlayerManager.instance.HasBoughtBuildingMap(index))
        {
            activeInfoPanel.alpha = 1f;
            activeInfoPanel.interactable = true;
            activeInfoPanel.blocksRaycasts = true;

            toBuyInfoPanel.alpha = 0f;
            toBuyInfoPanel.interactable = false;
            toBuyInfoPanel.blocksRaycasts = false;

            GetComponent<Image>().sprite = availableBuilding;
            RefreshBuyButton(true);
        }
        else
        {
            activeInfoPanel.alpha = 0f;
            activeInfoPanel.interactable = false;
            activeInfoPanel.blocksRaycasts = false;

            toBuyInfoPanel.alpha = 1f;
            toBuyInfoPanel.interactable = true;
            toBuyInfoPanel.blocksRaycasts = true;

            GetComponent<Image>().sprite = disableBuilding;
            RefreshBuyButton(false);
        }
    }

    public void RefreshBuyButton(bool isActive)
    {
        if (isActive)
        {
            buyButton.image.sprite = availableBuyButton;
        }
        else
        {
            buyButton.image.sprite = disableBuyButton;
        }
    }

    public void SetBuildingTitleTextValue()
    {
        buildingAvailableTitle.text = LocalizationManager.instance.StringForKey(nameTag);
        buildingDisableTitle.text = LocalizationManager.instance.StringForKey(nameTag);
    }

    public void SetCostOfBuilding()
    {
        buildingCostText.text = NumberFormatter.ToString(PlayerManager.instance.GetBuildingMapCost(index), true, true);
    }

    public void SetNumberOfFloorsAndCashPerSecondTextValue(int index)
    {
        int numberOfFloors = 0;
        float totalCashPerSecond = 0;

        for (int i = (index + 1) * 10 - 10; i < (index + 1) * 10; i++)
        {
            if (GameManager.instance.panels[i].GetComponent<SlotPanel>() != null)
            {
                numberOfFloors++;
            }
            Slot slot = PlayerManager.instance.GetSlot(i);
            if (slot.isUnlocked && PlayerManager.instance.HasBoughtManager(i))
            {
                totalCashPerSecond += slot.cashPerSecond;
            }
        }

        buildingNumberOfAvailableFloors.text = LocalizationManager.instance.StringForKey("NumberOfFloorsText") + " " + numberOfFloors + "/10";
        buildingCashPerSeconds.text = NumberFormatter.ToString(totalCashPerSecond * 60, true, false) + "/min";
    }

    public void OnBuyButton_Pressed()
    {
        if(PlayerManager.instance.cash >= PlayerManager.instance.GetBuildingMapCost(index))
        {
            ConfirmationPopup.afterConfirmationDelegate = DelegateAfterConfirmation_BuyNewBuilding;
            StartCoroutine(FindObjectOfType<MapPopup>().confirmationPopup.CallConfirmationPanel("ConfirmationPanel_BuyNewBuilding"));
        }
    }

    public void DelegateAfterConfirmation_BuyNewBuilding()
    {
        PlayerManager.instance.DecrementCashBy((float)PlayerManager.instance.GetBuildingMapCost(index));
        PlayerManager.instance.BoughtBuildingMap(index);

        RefreshUI();
    }
}
