using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(fileName ="UpgreadeData", menuName ="Game Data/Upgreade Data", order =1)]
public class UpgreadeData : ScriptableObject
{
    public int numberOfBuilding;

    [SerializeField]
    private string nameKey;

    public enum UpgreadeType
    {
        OnlyThisSlot,
        ForAllSlots
    }
    public UpgreadeType type;
    public Slot.Type slot;
    public float profitMultiplier;
    public float cost;
    public float costByGold;

    private void OnValidate()
    {
        Assert.IsTrue(nameKey != "");
        Assert.IsTrue(profitMultiplier > 0);
        Assert.IsTrue(cost > 0);
    }

    new public string name
    {
        get { return nameKey; }
    }

    public string description
    {
        get
        {
            string slotName = slot.ToString();
            string[] words = slotName.Split('_');
            string correctSlotName = words[0];

            if (type == UpgreadeType.OnlyThisSlot)
            {
                return LocalizationManager.instance.StringForKey("UpgradePanel_Value") + " " + LocalizationManager.instance.StringForKey(correctSlotName.ToUpper()) + " x" + profitMultiplier;
            }
            else
            {
                return LocalizationManager.instance.StringForKey("UpgradePanel_UpgradeAll") + " " + profitMultiplier;
            }
        }
    }
}
