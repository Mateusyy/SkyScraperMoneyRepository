using UnityEngine;

[CreateAssetMenu(fileName ="SlotData", menuName ="Game Data/Slot Data", order =1)]
public class SlotData : ScriptableObject
{
    public Sprite icon;
    public Color floorColor;
    public float timerToUnlock;
    public int numberOfBuilding;
    public Slot.Type type;
    public bool isUnlocked;
    public float valueOfSpeedUp;
    public int maxLevel;

    //Cost to unlock the slot
    public float initialCost;

    //multiplier to rised cost for upgreade
    [Header("MUST BE > THAN 1")]
    public float costMultiplier;
    [Space(10)]

    public float initialTime;
    public float timeMultiplier = 0.97264f;

    public float initialProfit;

    [System.Serializable]
    public struct MilestoneMultiplier
    {
        public int level;
        public float multiplier;
    }
    public MilestoneMultiplier[] milestoneMultipliers;
    public InteriorPanel interior;
}
