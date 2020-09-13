using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UpgreadePanel : MonoBehaviour
{
    [SerializeField]
    private int index;  //equal to arg in InitializeFunc() [need to invoke AddListenerToBuyButton in UpdateUI in GameManager.cs]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Image symbolOfSlotImage;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Text costText;
    public float cost;
    [SerializeField]
    private Image costIcon;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Sprite availableButtonSprite;
    [SerializeField]
    private Sprite dissableButtonSprite;
    [SerializeField]
    private Text buyButton_text;

    [SerializeField]
    private Button buyByGoldButton;
    [SerializeField]
    private Text buyByGoldButton_text;

    private UpgreadeData data;
    private bool haveListener = false;
    private bool haveListenerByGold = false;
    public int numberOfBuilding;

    public void Initialize(int upgreadeIndex)
    {
        Assert.IsFalse(PlayerManager.instance.HasBoughtUpgreade(upgreadeIndex));

        data = GameData.instance.GetDataForUpgreade(upgreadeIndex);
        numberOfBuilding = data.numberOfBuilding;

        index = upgreadeIndex;

        if(index % 2 == 0)
        {
            backgroundImage.sprite = backgroundSprites[0];
            nameText.color = GameColors.managerOrUpgradeSlot_TitleColor_v1;
        }
        else
        {
            backgroundImage.sprite = backgroundSprites[1];
            nameText.color = GameColors.managerOrUpgradeSlot_TitleColor_v2;
        }

        //image.sprite = data.image;
        nameText.text = data.name;
        symbolOfSlotImage.sprite = GameManager.instance.slotPanelSymbols[index % 10];
        descriptionText.text = data.description;
        cost = data.cost;
        buyButton_text.text = LocalizationManager.instance.StringForKey("UpgradesPanel_BuyButtonText");

        buyByGoldButton_text.text = NumberFormatter.ToString(number: data.costByGold, showDecimalPlaces: false, showDollarSign: false);
        costText.text = NumberFormatter.ToString(number: data.cost, showDecimalPlaces: false);

        bool canBuyUpgreade = PlayerManager.instance.cash >= data.cost;
        haveListener = false;
        haveListenerByGold = false;

        //button formater
        RefreshBuyButtonStatus(canBuyUpgreade);
    }

    public void RefreshBuyButtonStatus(bool canBuy)
    {
        AddListenerToBuyByGoldButton();
        if (canBuy && GameManager.instance.panels[(int)data.slot].GetComponent<SlotPanel>() != null)
        {
            buyButton.image.sprite = availableButtonSprite;
            AddListenerToBuyButton();
        }
        else
        {
            buyButton.image.sprite = dissableButtonSprite;
        }
    }

    public void AddListenerToBuyByGoldButton()
    {
        if (haveListenerByGold == false)
        {
            haveListenerByGold = true;
            buyByGoldButton.onClick.AddListener(() =>
            {
                if (PlayerManager.instance.gold >= data.costByGold)
                {
                    buyButton.interactable = false;
                    PlayerManager.instance.BoughtUpgreade(index);
                    PlayerManager.instance.DecrementGoldBy(data.costByGold);
                }
                else
                {
                    FindObjectOfType<ShopPopup>().ShowPopup();
                    return;
                }
                int slotIndex = (int)data.slot;
                PlayerManager.instance.GetSlot(slotIndex).UpdateUpgreadeProfitMultiplier(data.profitMultiplier);

                haveListenerByGold = false;
                Destroy(gameObject);

                GameManager.instance.OnUpdateUI();
            });
        }
    }

    public void AddListenerToBuyButton()
    {
        if (haveListener == false)
        {
            haveListener = true;
            buyButton.onClick.AddListener(() =>
            {
                buyButton.interactable = false;
                PlayerManager.instance.BoughtUpgreade(index);
                PlayerManager.instance.DecrementCashBy(data.cost);
                
                int slotIndex = (int)data.slot;
                PlayerManager.instance.GetSlot(slotIndex).UpdateUpgreadeProfitMultiplier(data.profitMultiplier);
                
                haveListener = false;
                Destroy(gameObject);

                GameManager.instance.OnUpdateUI();
            });
        }
    }
}