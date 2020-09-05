using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ManagerPanel : MonoBehaviour
{
    [SerializeField]
    private int index;
    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private Sprite[] backgroundSprites;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descriptionText;
    [SerializeField]
    private Text costText;
    public float cost;
    [SerializeField]
    private Image costImage;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Sprite availableButtonSprite;
    [SerializeField]
    private Sprite dissableButtonSprite;
    [SerializeField]
    private Text buyButton_text;
    [SerializeField]
    private Image symbolOfSlotImage;

    [SerializeField]
    private Button buyByGoldButton;
    [SerializeField]
    private Text buyByGoldButton_text;

    private ManagerData data;
    private bool haveListener = false;
    private bool haveListenerGoldBuy = false;
    public bool canBuyManager;
    public int numberOfBuilding;

    public void Initialize(int managerIndex)
    {
        Assert.IsFalse(PlayerManager.instance.HasBoughtManager(managerIndex));
        data = GameData.instance.GetDataForManager(managerIndex);
        numberOfBuilding = data.numberOfBuilding;

        index = managerIndex;

        if (index % 2 == 0)
        {
            backgroundImage.sprite = backgroundSprites[0];
            nameText.color = GameColors.managerOrUpgradeSlot_TitleColor_v1;
        }
        else
        {
            backgroundImage.sprite = backgroundSprites[1];
            nameText.color = GameColors.managerOrUpgradeSlot_TitleColor_v2;
            nameText.color = GameColors.managerOrUpgradeSlot_TitleColor_v2;
        }

        image.sprite = data.image;
        nameText.text = data.name;
        symbolOfSlotImage.sprite = GameManager.instance.slotPanelSymbols[index % 10];
        //descriptionText.text = LocalizationManager.instance.StringForKey(data.description);
        descriptionText.text = data.description;
        cost = data.cost;

        buyButton_text.text = LocalizationManager.instance.StringForKey("ManagersPanel_HireButtonText");
        costText.text = NumberFormatter.ToString(number: data.cost, showDecimalPlaces: false);

        buyByGoldButton_text.text = NumberFormatter.ToString(number: data.costByGold, showDecimalPlaces: false, showDollarSign: false);

        canBuyManager = PlayerManager.instance.cash >= data.cost;
        haveListener = false;
        haveListenerGoldBuy = false;

        //Format buyButton
        RefreshBuyButtonStatus(canBuyManager);
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
        if (haveListenerGoldBuy == false)
        {
            haveListenerGoldBuy = true;
            buyByGoldButton.onClick.AddListener(() =>
            {
                if (PlayerManager.instance.gold >= data.costByGold)
                {
                    buyButton.interactable = false;
                    PlayerManager.instance.BoughtManager(index);
                    PlayerManager.instance.DecrementGoldBy(data.costByGold);
                }
                else
                {
                    FindObjectOfType<ShopPopup>().ShowPopup();
                    return;
                }

                int slotIndex = (int)data.slot;
                //manager icon after buy manager
                if (GameManager.instance.panels[slotIndex].GetComponent<SlotPanel>() != null)
                {
                    (GameManager.instance.panels[slotIndex] as SlotPanel).SetVisibleManagerIcon();
                }

                if (data.type == ManagerData.ManagerType.AutoSlot)
                {
                    Assert.IsFalse(PlayerManager.instance.GetSlot(slotIndex).hasManager);
                    PlayerManager.instance.GetSlot(slotIndex).AssignManager(Slot.ManagerType.AutoRun);

                    if (data.showCashPerSecond)
                    {
                        PlayerManager.instance.GetSlot(slotIndex).SetShouldShowCashPerSecond(true);
                    }
                }
                else if (data.type == ManagerData.ManagerType.ReduceCost)
                {
                    PlayerManager.instance.GetSlot(slotIndex).AssignManager(Slot.ManagerType.ReduceCost);
                    PlayerManager.instance.GetSlot(slotIndex).SetCostReductionMultiplier(data.costReductionMultiplier);
                    if (data.showCashPerSecond)
                    {
                        PlayerManager.instance.GetSlot(slotIndex).SetShouldShowCashPerSecond(true);
                    }
                }

                haveListenerGoldBuy = false;
                Destroy(gameObject);
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
                PlayerManager.instance.BoughtManager(index);
                PlayerManager.instance.DecrementCashBy(data.cost);
                
                int slotIndex = (int)data.slot;
                //manager icon after buy manager
                if (GameManager.instance.panels[slotIndex].GetComponent<SlotPanel>() != null)
                {
                    (GameManager.instance.panels[slotIndex] as SlotPanel).SetVisibleManagerIcon();
                }

                if (data.type == ManagerData.ManagerType.AutoSlot)
                {
                    Assert.IsFalse(PlayerManager.instance.GetSlot(slotIndex).hasManager);
                    PlayerManager.instance.GetSlot(slotIndex).AssignManager(Slot.ManagerType.AutoRun);

                    if (data.showCashPerSecond)
                    {
                        PlayerManager.instance.GetSlot(slotIndex).SetShouldShowCashPerSecond(true);
                    }
                }
                else if (data.type == ManagerData.ManagerType.ReduceCost)
                {
                    PlayerManager.instance.GetSlot(slotIndex).AssignManager(Slot.ManagerType.ReduceCost);
                    PlayerManager.instance.GetSlot(slotIndex).SetCostReductionMultiplier(data.costReductionMultiplier);
                    if (data.showCashPerSecond)
                    {
                        PlayerManager.instance.GetSlot(slotIndex).SetShouldShowCashPerSecond(true);
                    }
                }

                haveListener = false;
                Destroy(gameObject);
            });
        }
    }
}