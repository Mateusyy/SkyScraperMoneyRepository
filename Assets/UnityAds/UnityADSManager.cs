using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class UnityADSManager : MonoBehaviour, IUnityAdsListener
{
    public static UnityADSManager instance;

    string gameId = "3578618";

    public string myPlacementId_rewardedVideo = "VideoBooster_x2";
    public string myPlacementId_doubleOfflineEarning = "DoubleOfflineEarning";
    public string myPlacementId_openOnesOfBottomPanel = "OpenOnesBottomPanel";

    bool testMode = false;

    public int indexOfSlotForBenefits;
    public enum BoosterType
    {
        x2,
        extraCash,
        doubleOfflineEarning,
        randomBonus,
        cutTimerToUnlockSlot,
        none
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this as UnityADSManager;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            gameId = "3469387";

        if (Application.platform == RuntimePlatform.Android)
            gameId = "3578618";

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, false);
    }

    public void ShowRewardedVideo(BoosterType boosterType)
    {
        GameManager.pauseBeacuseADS = true;

        PlayerManager.instance.boosterType = boosterType;
        Advertisement.Show(myPlacementId_rewardedVideo);
    }

    public void ShowBottomAdvert()
    {
        Advertisement.Show();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if(PlayerManager.instance.boosterType == BoosterType.x2 && 
                placementId == myPlacementId_rewardedVideo && 
                !PlayerManager.instance.isBooster)
            {
                GameManager.instance.BoosterX2_ActionAfterFinishedAdvert();
            }

            if (PlayerManager.instance.boosterType == BoosterType.extraCash &&
                placementId == myPlacementId_rewardedVideo)
            {
                GameManager.instance.BoosterExtraCash_ActionAfterFinishedAdvert();
            }

            if(PlayerManager.instance.boosterType == BoosterType.doubleOfflineEarning &&
                placementId == myPlacementId_rewardedVideo)
            {
                GameManager.instance.DoubleOfflineEarning_ActionAfterFinishedAdvert();
            }

            if (PlayerManager.instance.boosterType == BoosterType.randomBonus &&
                placementId == myPlacementId_rewardedVideo)
            {
                GameManager.instance.RandomBonus_ActionAfterFinishedAdvert();
            }

            if (PlayerManager.instance.boosterType == BoosterType.cutTimerToUnlockSlot &&
                placementId == myPlacementId_rewardedVideo)
            {
                GameManager.instance.CutTimerToUnlockSlot_ActionAfterFinishedAdvert(indexOfSlotForBenefits);
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            //
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == myPlacementId_rewardedVideo)
        {
            //myButton.interactable = true;
        }
    }
}
