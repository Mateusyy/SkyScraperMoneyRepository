using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Advertisements;

using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.UnityAds;

using System;

public class UnityADSManager : MonoBehaviour//, IUnityAdsListener
{
    public static UnityADSManager instance;
    public int indexOfSlotForBenefits;

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

    public enum BoosterType
    {
        x2,
        extraCash,
        doubleOfflineEarning,
        randomBonus,
        cutTimerToUnlockSlot,
        none
    }

    private RewardedAd rewardedAd;

    private void Start()
    {
        List<string> deviceIds = new List<string>();
        deviceIds.Add("5ED557A5AF39EFAE351F90661FACB3CD");

        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();

        MobileAds.Initialize(initStatus => { });
        MobileAds.SetRequestConfiguration(requestConfiguration);
        
        this.rewardedAd = new RewardedAd("ca-app-pub-1822284172999308/7197718228");

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd.LoadAd(request);
    }

    public void CreateAndLoadRewardedAd()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1822284172999308/7197718228";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            string adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.CreateAndLoadRewardedAd();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (PlayerManager.instance.boosterType == BoosterType.x2 && !PlayerManager.instance.isBooster)
        {
            GameManager.instance.BoosterX2_ActionAfterFinishedAdvert();
        }

        if (PlayerManager.instance.boosterType == BoosterType.extraCash)
        {
            GameManager.instance.BoosterExtraCash_ActionAfterFinishedAdvert();
        }

        if (PlayerManager.instance.boosterType == BoosterType.doubleOfflineEarning)
        {
            GameManager.instance.DoubleOfflineEarning_ActionAfterFinishedAdvert();
        }

        if (PlayerManager.instance.boosterType == BoosterType.randomBonus)
        {
            GameManager.instance.RandomBonus_ActionAfterFinishedAdvert();
        }

        if (PlayerManager.instance.boosterType == BoosterType.cutTimerToUnlockSlot)
        {
            GameManager.instance.CutTimerToUnlockSlot_ActionAfterFinishedAdvert(indexOfSlotForBenefits);
        }
    }

    public void ShowRewardedVideo(BoosterType boosterType)
    {
        GameManager.pauseBeacuseADS = true;
        PlayerManager.instance.boosterType = boosterType;

        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }


    /*string gameId = "3578618";

    public string myPlacementId_rewardedVideo = "VideoBooster_x2";
    public string myPlacementId_doubleOfflineEarning = "DoubleOfflineEarning";
    public string myPlacementId_openOnesOfBottomPanel = "OpenOnesBottomPanel";

    public int indexOfSlotForBenefits;

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
    }*/
}
