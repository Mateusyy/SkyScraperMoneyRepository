using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class Slot
{
    public delegate void EventHandler();

    [field: System.NonSerialized]
    public EventHandler OnUnitProduced;
    [field: System.NonSerialized]
    public EventHandler OnManagerHired;

    public enum Type
    {
        //First building
        Bar,
        Shop,
        Cinema,
        Factory,
        Library,
        Office,
        Icecream,
        Gym,
        Radio,
        Lab,
        //second building
        Bar_2,
        Shop_2,
        Cinema_2,
        Factory_2,
        Library_2,
        Office_2,
        Icecream_2,
        Gym_2,
        Radio_2,
        Lab_2,
        //third building
        b3_1,
        b3_2,
        b3_3,
        b3_4,
        b3_5,
        b3_6,
        b3_7,
        b3_8,
        b3_9,
        b3_10,
        //fourth building
        b4_1,
        b4_2,
        b4_3,
        b4_4,
        b4_5,
        b4_6,
        b4_7,
        b4_8,
        b4_9,
        b4_10,
        //fifth building
        b5_1,
        b5_2,
        b5_3,
        b5_4,
        b5_5,
        b5_6,
        b5_7,
        b5_8,
        b5_9,
        b5_10,
        //sixth building
        b6_1,
        b6_2,
        b6_3,
        b6_4,
        b6_5,
        b6_6,
        b6_7,
        b6_8,
        b6_9,
        b6_10,
        //seventh building
        b7_1,
        b7_2,
        b7_3,
        b7_4,
        b7_5,
        b7_6,
        b7_7,
        b7_8,
        b7_9,
        b7_10,
        //eight building
        b8_1,
        b8_2,
        b8_3,
        b8_4,
        b8_5,
        b8_6,
        b8_7,
        b8_8,
        b8_9,
        b8_10,
        //nineth building
        b9_1,
        b9_2,
        b9_3,
        b9_4,
        b9_5,
        b9_6,
        b9_7,
        b9_8,
        b9_9,
        b9_10,
        //tenth building
        b10_1,
        b10_2,
        b10_3,
        b10_4,
        b10_5,
        b10_6,
        b10_7,
        b10_8,
        b10_9,
        b10_10,
    }

    public Type type { get; private set; }

    public string name { get { return type.ToString(); } }
    public int index { get { return (int)type; } }
    private SlotData data
    {
        get { return GameData.instance.GetDataForSlot(index); }
    }
    public Sprite image { get { return data.icon; } }
    public Color floorColor { get { return data.floorColor; } }
    public int level { get; private set; }
    public bool[] hasUnlockedElemenet { get; set; } 
    public int maxLevel { get { return data.maxLevel; } }
    public float costToUnlock { get { return data.initialCost; } }
    public float timerToUnlock { get { return data.timerToUnlock; } }
    public bool isUnlocked { get; private set; }
    public float valueOfSpeedUp { get; private set; }
    public int numberOfBuilding { get; set; }

    public enum ManagerType
    {
        None,
        AutoRun,
        ReduceCost
    }
    private ManagerType managerType;
    public bool hasManager { get { return false; } }
    public int bulkLevelUpIndex = 0;
    public float currentCostValue;

    //Constructor
    public Slot(Type type)
    {
        this.type = type;
        milestoneProfitMultiplier = upgradeProfitMultiplier = contractProfitMultiplier = adBoosterMultiplier = 1;
        Reset();
    }

    public void Reset()
    {
        level = 1;

        milestoneProfitMultiplier = upgradeProfitMultiplier = 1;
        SetCostReductionMultiplier(0);

        isUnlocked = data.isUnlocked;
        valueOfSpeedUp = data.valueOfSpeedUp;
        hasRechedMilestone = new bool[data.milestoneMultipliers.Length]; //defaults to false
        hasUnlockedElemenet = new bool[data.interior.interiorObjects.Count]; //defaults to false
        numberOfBuilding = data.numberOfBuilding;

        ManagerStopWorking(); //prestige
        shouldShowCashPerSecond = false;
        AssignManager(ManagerType.None);
        timeToProduceThisUnit = -1;
        isProducing = false;
        bulkLevelUpIndex = 0;
        currentCostValue = CalculateCurrentCostValue();
        timer = 0;
    }

    #region Time

    public float timeToProduce
    {
        get
        {
            float temp = data.initialTime * Mathf.Pow(data.timeMultiplier, level);
            return temp > 0 ? temp : 0.01f;
        }
    }

    public float timeSpeedAfterEachUpgrade
    {
        get
        {
            return 1f - data.timeMultiplier;
        }
    }

    private float timeToProduceThisUnit;
    [System.NonSerialized]
    public float timer = 0;

    public float unitCompletePercentage
    {
        get { return timer > 0 ? (timeToProduceThisUnit - timer) / timeToProduceThisUnit : 0; }
    }

    public string timeDisplayString
    {
        get
        {
            System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(timer);

            int hours = timeSpan.Days > 0 ? timeSpan.Days * 24 + timeSpan.Hours : timeSpan.Hours;

            return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    #endregion

    #region Cost

    private float costReductionMultiplier;

    public void SetCostReductionMultiplier(float costReductionMultiplier)
    {
        Assert.IsTrue(costReductionMultiplier >= 0 && costReductionMultiplier < 1, string.Format("costReductionMultiplier: {0} should be >= 0 && < 1", costReductionMultiplier));
        this.costReductionMultiplier = costReductionMultiplier;
    }

    private float costReductionFactor
    {
        get { return 1 - costReductionMultiplier; }
    }

    private float CostToUpgreadeByLevels(int numberOfLevels)
    {
        //NAN
        return data.initialCost * ((Mathf.Pow(data.costMultiplier, level) * (Mathf.Pow(data.costMultiplier, numberOfLevels) - 1)) / (data.costMultiplier - 1)) * costReductionFactor;
    }

    public int DetermineMaximumNumberOfLevelsPlayerCanUpgrade()
    {
        return Mathf.FloorToInt(Mathf.Log((((PlayerManager.instance.cash * (data.costMultiplier - 1)) / (data.initialCost * costReductionFactor * Mathf.Pow(data.costMultiplier, level))) + 1), data.costMultiplier));
    }

    #endregion

    #region Profit

    private float milestoneProfitMultiplier;

    private float MilestoneProfitMultiplierForLevel(int level)
    {
        float milestoneProfitMultiplier = 1;
        for (int i = 0; i < data.milestoneMultipliers.Length; i++)
        {
            if(level >= data.milestoneMultipliers[i].level)
            {
                if(i <= data.interior.interiorObjects.Count)
                {
                    milestoneProfitMultiplier *= data.milestoneMultipliers[i].multiplier;
                }
            }
        }
        return milestoneProfitMultiplier;
    }

    private void UpdateMilestoneProfitMultiplier(float amount)
    {
        Assert.IsTrue(amount >= 1, string.Format("amount: {0} should be >= 1", amount));
        milestoneProfitMultiplier = amount;
    }

    private float upgradeProfitMultiplier;

    public void UpdateUpgreadeProfitMultiplier(float amount)
    {
        Assert.IsTrue(amount >= 1, string.Format("amount: {0} should be >= 1", amount));
        upgradeProfitMultiplier *= amount;
    }

    private float contractProfitMultiplier;

    public void UpdateContractProfitMultiplier(float amount)
    {
        Assert.IsTrue(amount >= 1);
        contractProfitMultiplier *= amount;
    }

    private float adBoosterMultiplier;

    public void UpdateAdBoosterMultiplier(float amount)
    {
        Assert.IsTrue(amount >= 1);
        adBoosterMultiplier = amount;
    }

    public float profit
    {
        get { return data.initialProfit * level * milestoneProfitMultiplier * upgradeProfitMultiplier * contractProfitMultiplier * adBoosterMultiplier; }
    }

    public float ProfitForLevel(int level)
    {
        return data.initialProfit * level * MilestoneProfitMultiplierForLevel(level) * upgradeProfitMultiplier * contractProfitMultiplier * adBoosterMultiplier;
    }

    public float cashPerSecond
    {
        get { return profit / timeToProduce; }
    }

    public float CashPerSecondForLevel(int level)
    {
        return ProfitForLevel(level) / timeToProduce;
    }

    public bool shouldShowCashPerSecond { get; private set; }

    public void SetShouldShowCashPerSecond(bool shouldShowCashPerSecond)
    {
        this.shouldShowCashPerSecond = shouldShowCashPerSecond;
    }

    #endregion

    #region PRODUCING

    public bool isProducing { get; set; }

    public void StartProducingOrSpeedUp()
    {
        if(isProducing == false)
        {
            MS.instance.StartCoroutine(ProduceUnit());
        }
        else
        {
            GameManager.instance.panels[index].GetComponent<SlotPanel>().TurnSpeedUpEffect();
            GameManager.instance.panels[index].GetComponent<SlotPanel>().TurnSpeedUpSound();
            timer -= valueOfSpeedUp;
        }
    }

    private IEnumerator ProduceUnit()
    {
        isProducing = true;

        timeToProduceThisUnit = timeToProduce;
        //start timer
        timer = timeToProduceThisUnit;
        while ((timer -= Time.deltaTime) > 0)
        {
            GameManager.instance.OnUpdateUI();
            yield return null;
        }
        //unit produced
        if (OnUnitProduced != null)
        {
            OnUnitProduced();
        }

        if (isProducing)
        {
            PlayerManager.instance.IncrementCashBy((float)profit);
            PlayerManager.instance.IncrementSlotsCounter(index, 1);
        }
        
        GameManager.instance.OnUpdateUI();
        timer = 0;
        isProducing = false;
    }

    #endregion

    #region Manager

    [System.NonSerialized]
    private Coroutine managerStartsWorkingCorutine;

    public void AssignManager(ManagerType managerType)
    {
        if(this.managerType == ManagerType.None && managerType == ManagerType.AutoRun)
        {
            if (OnManagerHired != null)
                OnManagerHired();
        }

        this.managerType = managerType;
        if (isUnlocked)
        {
            if(managerType == ManagerType.AutoRun)
            {
                managerStartsWorkingCorutine = MS.instance.StartCoroutine(ManagerStartsWorkingCorutine());
            }
        }
    }

    public void ManagerStartsWorking()
    {
        Assert.IsTrue(PlayerManager.instance.HasBoughtManager(index));
        managerStartsWorkingCorutine = MS.instance.StartCoroutine(ManagerStartsWorkingCorutine());
    }

    private IEnumerator ManagerStartsWorkingCorutine()
    {
        while (true)
        {
            if (timer > 0)  //now is producing
                yield return null;
            else
                yield return ProduceUnit();
        }
    }

    public void ManagerStopWorking()
    {
        if(managerStartsWorkingCorutine != null)
        {
            MS.instance.StopCoroutine(managerStartsWorkingCorutine);
        }
    }

    #endregion

    #region Level

    public bool upgreadeLevelExists { get { return level < maxLevel; } }
    private int numberOfLevelToUpgreadeBy;
    private float costToUpgreadeByXLevels;

    public float UpgreadeXLevelsCost(int numberOfLevel)
    {
        numberOfLevelToUpgreadeBy = (level + numberOfLevel > maxLevel ? maxLevel - level : numberOfLevel);

        if (numberOfLevelToUpgreadeBy > 0)
        {
            costToUpgreadeByXLevels = CostToUpgreadeByLevels(numberOfLevelToUpgreadeBy);
            return costToUpgreadeByXLevels;
        }
        else
        {
            Debug.LogError("numberOfLevelsToUpgreade == 0");
            return -1;
        }
    }

    public float UpgreadeMaxLevelsCost()
    {
        int numberOfLevels = DetermineMaximumNumberOfLevelsPlayerCanUpgrade();
        return numberOfLevels == 0 && upgreadeLevelExists ? UpgreadeXLevelsCost(1) : UpgreadeXLevelsCost(numberOfLevels);
    }

    public bool UpgreadeToLastCalculateLevel()
    {
        if(numberOfLevelToUpgreadeBy > 0 && PlayerManager.instance.cash >= costToUpgreadeByXLevels)
        {
            UpdateLevelBy(numberOfLevelToUpgreadeBy);

            PlayerManager.instance.DecrementCashBy(costToUpgreadeByXLevels);
            GameManager.instance.OnUpdateUI();
            //reset cached data
            costToUpgreadeByXLevels = -1;
            numberOfLevelToUpgreadeBy = -1;

            return true;
        }

        return false;
    }

    public void UpdateLevelBy(int amount)
    {
        Assert.IsTrue(level + amount <= maxLevel);
        level += amount;

        for (int i = 0; i < hasRechedMilestone.Length; i++)
        {
            if (!hasRechedMilestone[i])
            {
                if(level >= data.milestoneMultipliers[i].level)
                {
                    if(i < hasUnlockedElemenet.Length)
                    {
                        if (hasUnlockedElemenet[i])
                        {
                            hasRechedMilestone[i] = true;
                            UpdateMilestoneProfitMultiplier(data.milestoneMultipliers[i].multiplier);
                        }
                    }
                    else
                    {
                        hasRechedMilestone[i] = true;
                        UpdateMilestoneProfitMultiplier(data.milestoneMultipliers[i].multiplier);
                    }
                }
            }
        }
    }

    private bool[] hasRechedMilestone;

    public float nextMilestonePercentage
    {
        get
        {
            for (int i = 0; i < data.milestoneMultipliers.Length; i++)
            {
                int j = i + 1;
                if (!hasRechedMilestone[i])
                {
                    return 1 - (data.milestoneMultipliers[i].level - level * 1f) / (data.milestoneMultipliers[i].level - 1);
                }
                else if (!hasRechedMilestone[j])
                {
                    return 1 - (data.milestoneMultipliers[j].level - level * 1f) / (data.milestoneMultipliers[j].level - data.milestoneMultipliers[i].level);
                }
            }
            return 0;
        }
    }

    public void SetUnlocked()
    {
        isUnlocked = true;
        if (hasManager)
        {
            Debug.Log("Block");
        }
    }

    #endregion

    public float CalculateCurrentCostValue()
    {
        return currentCostValue = bulkLevelUpIndex < 3
            ?
            UpgreadeXLevelsCost(Constant.BULK_UPGRADE_LEVELS[bulkLevelUpIndex]) :
            UpgreadeMaxLevelsCost();
    }

    /*ITEMS TO UNLOCK FOR EVERY FLOOR*/

    public int GetMilestoneLevelTarget(int index)
    {
        return data.milestoneMultipliers[index].level;
    }

    public float GetMilestoneValue(int index)
    {
        return data.milestoneMultipliers[index].multiplier;
    }

    public void SetUnlockedObject(int index)
    {
        if(index > hasUnlockedElemenet.Length)
        {
            Debug.LogError("Index is out of range.");
            return;
        }

        hasUnlockedElemenet[index] = true;
    }

    public bool GetInformationsAboutObjectToUnlock(int index)
    {
        return hasUnlockedElemenet[index];
    }

    /*===============================*/
}
