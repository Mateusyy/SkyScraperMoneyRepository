using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ManagerData", menuName ="Game Data/Manager Data", order =1)]
public class ManagerData : ScriptableObject
{
    public Sprite image;
    public Sprite imageForSlot;
    public int numberOfBuilding;
    [SerializeField]
    private string nameKey;
    public enum ManagerType
    {
        AutoSlot,
        ReduceCost
    }
    public ManagerType type;
    public Slot.Type slot;
    public bool showCashPerSecond;

    [Range(0, 1)]
    public float costReductionMultiplier;
    public float cost;
    public float costByGold;

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

            if(type == ManagerType.AutoSlot)
            {
                return LocalizationManager.instance.StringForKey("ManagerData_Runs") + " " + correctSlotName;
            }
            else
            {
                return correctSlotName + " " + LocalizationManager.instance.StringForKey("ManagerData_Discount") + " -" + costReductionMultiplier * 100 + "%"; 
            }
        }
    }
}
