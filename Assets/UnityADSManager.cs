using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Api;

using System;
using Firebase.Analytics;

public class UnityADSManager : MonoBehaviour
{
    public static UnityADSManager instance;

    private BannerView bannerView;
    private RewardedAd rewardedAd;

    [HideInInspector]
    public int indexOfSlotForBenefits;

    public bool banner = false;

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

    public void SetBannerVariable(bool value)
    {
        this.banner = value;

        MainUI mainUI = FindObjectOfType<MainUI>();
        if(banner == true && mainUI != null && mainUI.bottomPanel != null)
        {
            mainUI.SetHeightAndPosYForRectTransform(mainUI.bottomPanel, 300);
            GameManager.instance.UpdateMainViewWhenBannerIsActive();
            RequestBannerAd();
        }
        else
        {
            mainUI.SetHeightAndPosYForRectTransform(mainUI.bottomPanel, 160);
            DestroyBannerAd();
        }
    }

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

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();

        this.rewardedAd.LoadAd(request);
    }

    #region BANNER ADS

    public void RequestBannerAd()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1822284172999308/7903029881";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }

    #endregion

    #region REWARDED_AD

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
        switch (PlayerManager.instance.boosterType)
        {
            case BoosterType.x2:
                if (!PlayerManager.instance.isBooster)
                    GameManager.instance.BoosterX2_ActionAfterFinishedAdvert();
                break;

            case BoosterType.extraCash:
                GameManager.instance.BoosterExtraCash_ActionAfterFinishedAdvert();
                break;

            case BoosterType.doubleOfflineEarning:
                GameManager.instance.DoubleOfflineEarning_ActionAfterFinishedAdvert();
                break;

            case BoosterType.randomBonus:
                GameManager.instance.RandomBonus_ActionAfterFinishedAdvert();
                break;

            case BoosterType.cutTimerToUnlockSlot:
                GameManager.instance.CutTimerToUnlockSlot_ActionAfterFinishedAdvert(indexOfSlotForBenefits);
                break;

            case BoosterType.none:
                break;
            default:
                break;
        }
    }

    public void ShowRewardedVideo(BoosterType boosterType)
    {
        GameManager.pauseBeacuseADS = true;
        PlayerManager.instance.boosterType = boosterType;

        FirebaseAnalytics.LogEvent("Rewarded_Video_Started", new Parameter("booster_type", boosterType.ToString()));
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
    #endregion
}
