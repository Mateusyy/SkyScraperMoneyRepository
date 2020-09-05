using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairFinishPopup : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private Text countText;

    private int valueOfCost = 0;
    private BuySlotPanel buySlotPanel;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void CountGoldCost(float value)
    {
        if (value >= 0 && value <= 10) valueOfCost = 1;
        else if (value > 10 && value <= 30) valueOfCost = 2;
        else if (value > 30 && value <= 90) valueOfCost = 5;
        else if (value > 90 && value <= 300) valueOfCost = 30;
        else if (value > 300 && value <= 900) valueOfCost = 50;
        else if (value > 900 && value <= 1800) valueOfCost = 100;
        else if (value > 1800 && value <= 3600) valueOfCost = 250;
        else
        {
            valueOfCost = Mathf.FloorToInt(value/2);
        }
        countText.text = valueOfCost.ToString();
    }

    public void ShowPopup(BuySlotPanel buySlotPanel, float secondToEndCounting)
    {
        this.buySlotPanel = buySlotPanel;
        CountGoldCost(secondToEndCounting);
        anim.SetTrigger("Show");
    }

    public void FinishButton_OnPressed()
    {
        if(valueOfCost <= 0)
        {
            HidePopup();
            return;
        }

        if (PlayerManager.instance.gold >= valueOfCost)
        {
            buySlotPanel.afterBooster = true;
            PlayerManager.instance.DecrementGoldBy(valueOfCost);
            HidePopup();
            return;
        }
        else
        {
            FindObjectOfType<ShopPopup>().ShowPopup();
            return;
        }
    }

    public void HidePopup()
    {
        anim.SetTrigger("Hide");
    }
}
