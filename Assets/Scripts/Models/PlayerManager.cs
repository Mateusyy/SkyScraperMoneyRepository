using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[System.Serializable]
public class PlayerManager
{
    public float cash { get; private set; }
    public float gold { get; private set; }
    private long pointsToLeaderboard { get; set; }

    public int level;
    private Slot[] slots;
    private int[] slotsCounter;

    private bool[] managerIsBought;
    private float[] managersCost;

    private bool[] upgreadeIsBought;

    private float[] upgreadesCost;

    private bool[] buildingMapIsBought;
    private float[] buildingMapCost;

    public BuyButtonParamsToUnlock[] buyButtonParamsToUnlock;

    public System.DateTime dateLastPlayed;

    public UnityADSManager.BoosterType boosterType;

    public ulong lastBoosterStartDateTime;
    public bool isBooster = false;
    public bool isNotification = true; 

    public int extraCashBoosterCounter;

    public static PlayerManager _instance;
    public float maxTimeOfflineEarning;
    public float contractPrice;
    public float XP;

    public static PlayerManager instance
    {
        get {
            return _instance ?? (_instance = BinarySerializer.Load<PlayerManager>(typeof(PlayerManager).Name));
        }
    }

    #region Initialization

    public static void Create()
    {
        if (!BinarySerializer.FileExists(typeof(PlayerManager).Name))
        {
            _instance = (PlayerManager)System.Activator.CreateInstance(type: typeof(PlayerManager), nonPublic: true);
        }
    }

    protected PlayerManager()
    {
        //1 hour
        maxTimeOfflineEarning = 3600f;
        slots = new Slot[GameData.instance.numberOfSlots];
        slotsCounter = new int[GameData.instance.numberOfSlots];

        managersCost = new float[GameData.instance.numberOfManagers];
        upgreadesCost = new float[GameData.instance.numberOfUpgreades];

        buildingMapCost = new float[GameData.instance.numberOfBuildingMap];
        buildingMapIsBought = new bool[GameData.instance.numberOfBuildingMap];

        buyButtonParamsToUnlock = new BuyButtonParamsToUnlock[GameData.instance.numberOfSlots];
        
        //slots
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new Slot(GameData.instance.GetDataForSlot(i).type);
            slotsCounter[i] = 0;
        }

        //managers
        for (int i = 0; i < managersCost.Length; i++)
        {
            managersCost[i] = GameData.instance.GetDataForManager(i).cost;
        }

        //upgrades
        for (int i = 0; i < upgreadesCost.Length; i++)
        {
            upgreadesCost[i] = GameData.instance.GetDataForUpgreade(i).cost;
        }

        //building map
        for (int i = 0; i < buildingMapCost.Length; i++)
        {
            buildingMapCost[i] = GameData.instance.GetDataForBuildingMap(i).cost;
            buildingMapIsBought[i] = GameData.instance.GetDataForBuildingMap(i).isActive;
        }

        for (int i = 0; i < buyButtonParamsToUnlock.Length; i++)
        {
            buyButtonParamsToUnlock[i] = new BuyButtonParamsToUnlock();
        }
        Reset();
    }

    internal void ReloadData()
    {
        //managers
        for (int i = 0; i < managersCost.Length; i++)
        {
            managersCost[i] = GameData.instance.GetDataForManager(i).cost;
        }

        //upgrades
        for (int i = 0; i < upgreadesCost.Length; i++)
        {
            upgreadesCost[i] = GameData.instance.GetDataForUpgreade(i).cost;
        }

        //building map
        for (int i = 0; i < buildingMapCost.Length; i++)
        {
            buildingMapCost[i] = GameData.instance.GetDataForBuildingMap(i).cost;
        }
    }

    private void Reset()
    {
        cash = 0;
        gold = 50;
        pointsToLeaderboard = 0;
        level += 1;
        extraCashBoosterCounter = 3;
        boosterType = UnityADSManager.BoosterType.none;
        isNotification = true;

        managerIsBought = new bool[GameData.instance.numberOfManagers];
        upgreadeIsBought = new bool[GameData.instance.numberOfUpgreades];
        dateLastPlayed = System.DateTime.MinValue;

        Save();
    }

    private void ResetContract()
    {
        cash = 0;
        level += 1;
        XP = 0;
        boosterType = UnityADSManager.BoosterType.none;

        managerIsBought = new bool[GameData.instance.numberOfManagers];
        upgreadeIsBought = new bool[GameData.instance.numberOfUpgreades];
        dateLastPlayed = System.DateTime.MinValue;

        Save();
    }

    #endregion

    public Slot GetSlot(int index)
    {
        Assert.IsTrue(index >= 0 && index < slots.Length);
        return slots[index];
    }

    public float totalCashPerSecond
    {
        get
        {
            float totalCashPerSecond = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].isUnlocked && PlayerManager.instance.HasBoughtManager(i))
                {
                    totalCashPerSecond += slots[i].cashPerSecond;
                }
            }
            return totalCashPerSecond;
        }
    }

    public float DetermineEarningSinceLastPlay()
    {
        //Debug.Log("Have to do getting time from internet");
        float timeSince = (float)(DateTime.UtcNow - dateLastPlayed).TotalSeconds;

        if (timeSince >= maxTimeOfflineEarning)
        {
            timeSince = maxTimeOfflineEarning;
        }

        float earnings = totalCashPerSecond * timeSince * 0.1f;
        Debug.Log("In determine");
        return earnings;
    }

    public float ContractTargetFib(int level)
    {
        if ((level == 1) || (level == 2))
        {
            return 1;
        }
        else
        {
            return (ContractTargetFib(level - 1) + ContractTargetFib(level - 2));
        }
    }

    public bool shouldConsiderContract
    {
        get
        {
            contractPrice = ContractTargetFib(level + 1) * Constant.CONTRACT_TARGET_CONST;
            if (cash > contractPrice)
            {
                return true;
            }
            return false;
        }
    }

    public IEnumerator Contract()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].isProducing = false;
            slots[i].UpdateContractProfitMultiplier(1.2f);
            slots[i].Reset();
            slotsCounter[i] = 0;
        }
        DecrementCashBy(cash);

        ResetContract();
        dateLastPlayed = System.DateTime.UtcNow;

        GameManager.instance.OnUpdateUI();
        yield return new WaitForSeconds(1f);
    }

    public void AdBooster(bool status)
    {
        switch (status)
        {
            case true:
                isBooster = true;
                break;
            case false:
                isBooster = false;
                break;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (status)
            {
                slots[i].UpdateAdBoosterMultiplier(2);
            }
            else
            {
                slots[i].UpdateAdBoosterMultiplier(1);
            }
        }
    }

    public void BoughtManager(int index)
    {
        Assert.IsTrue(index >= 0 && index < managerIsBought.Length);
        managerIsBought[index] = true;
    }

    public void BoughtUpgreade(int index)
    {
        Assert.IsTrue(index >= 0 && index < upgreadeIsBought.Length);
        upgreadeIsBought[index] = true;
    }

    public void BoughtBuildingMap(int index)
    {
        Assert.IsTrue(index >= 0 && index < buildingMapIsBought.Length);
        buildingMapIsBought[index] = true;
    }

    public bool HasBoughtManager(int index)
    {
        Assert.IsTrue(index >= 0 && index < managerIsBought.Length);
        return managerIsBought[index];
    }

    public bool HasBoughtUpgreade(int index)
    {
        Assert.IsTrue(index >= 0 && index < upgreadeIsBought.Length);
        return upgreadeIsBought[index];
    }

    public bool HasBoughtBuildingMap(int index)
    {
        Assert.IsTrue(index >= 0 && index < buildingMapIsBought.Length);
        return buildingMapIsBought[index];
    }

    public float GetManagerCost(int index)
    {
        Assert.IsTrue(index >= 0 && index < managersCost.Length);
        return managersCost[index];
    }

    public float GetUpgreadeCost(int index)
    {
        Assert.IsTrue(index >= 0 && index < upgreadesCost.Length);
        return upgreadesCost[index];
    }

    public float GetBuildingMapCost(int index)
    {
        Assert.IsTrue(index >= 0 && index < buildingMapCost.Length);
        return buildingMapCost[index];
    }

    public void IncrementCashBy(float amount)
    {
        cash += amount; //Save();
    }

    public void DecrementCashBy(float amount)
    {
        cash -= amount; //Save();
        pointsToLeaderboard += 1;
        XP += (amount / 1000000000);
    }

    public void IncrementGoldBy(float amount)
    {
        gold += amount;
    }

    public void DecrementGoldBy(float amount)
    {
        gold -= amount; //Save();
    }

    public void IncrementSlotsCounter(int index, int amount)
    {
        slotsCounter[index] += amount; //Save();
    }

    public void DecrementSlotsCounter(int index, int amount)
    {
        slotsCounter[index] -= amount; //Save();
    }

    public int GetValueSlotsCounter(int index)
    {
        Assert.IsTrue(index >= 0 && index < slotsCounter.Length);
        return slotsCounter[index];
    }

    protected void Save()
    {
        BinarySerializer.Save(typeof(PlayerManager).Name, this);
    }

    public void OnSaveToDisk()
    {
        dateLastPlayed = System.DateTime.UtcNow;
        Save();
    }

    public void SetGamePaused(bool gamePaused)
    {
        //either start or stop the business managers
        for (int i = 0; i < slots.Length; i++)
        {
            if (gamePaused)
            {
                slots[i].ManagerStopWorking();
            }
            else if (!gamePaused && PlayerManager.instance.managerIsBought[i])
            {
                slots[i].ManagerStartsWorking();
            }
        }
        //save
        OnSaveToDisk();
    }
}
