using System;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using NiobiumStudios;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject firstTimeCanvas;
    [SerializeField]
    public GameObject tutorialPopup;

    [SerializeField]
    public ManagersPopup managersPopup;
    [SerializeField]
    private CanvasGroup managersNotification;
    [SerializeField]
    private CanvasGroup managersBlurBackground;

    [SerializeField]
    public OfficePopup officePopup;
    [SerializeField]
    private CanvasGroup officeNotification;
    [SerializeField]
    private CanvasGroup officeBlurBackground;

    [SerializeField]
    public UpgreadePopup upgreadePopup;
    [SerializeField]
    private CanvasGroup upgreadesNotification;
    [SerializeField]
    private CanvasGroup upgradesBlurBackground;

    [SerializeField]
    public OfflineEarning offlineEarningPopup;

    [SerializeField]
    private OptionsPopup optionsPopup;

    [SerializeField]
    private MapPopup mapPopup;

    [SerializeField]
    private DailyRewardsInterface dailyRewardsPopup;

    [SerializeField]
    private UpgradeEachFloorPopup upgradeEachFloorPopup;

    [SerializeField]
    public ScrollRect scrollView;
    [SerializeField]
    private RectTransform scrollViewContent;
    [SerializeField]
    private SlotPanel slotPanelPrefab;
    [SerializeField]
    private GameObject whenBannerActiveImagePrefab;
    public Sprite[] slotPanelSprites;
    public Sprite[] slotPanelSymbols;

    [SerializeField]
    private BuySlotPanel buySlotPanelPrefab;
    public Sprite[] buySlotPanelSprites;

    [SerializeField]
    private GameObject bottomListGameObject;
    public Sprite[] bottomListImages;
    [SerializeField]
    private GameObject topListGameObject;
    public Sprite[] topListImages;

    [SerializeField]
    private Text cashText;
    [SerializeField]
    private Text goldText;

    [SerializeField]
    private Button adBoosterButton;
    [SerializeField]
    private Text adBoosterTimerText;

    [SerializeField]
    private Text adBoosterExtraCashCounter;
    [SerializeField]
    private GameObject adBoosterExtraCashCounterPopup;
    [SerializeField]
    private GameObject IAPadBoosterExtraCashCounterPopup;
    [SerializeField]
    private GameObject extraCashGO;
    [SerializeField]
    private Text extraCashText;

    [SerializeField]
    private ContractPanel contract;
    [SerializeField]
    private Image BlockImage;

    public MonoBehaviour[] panels;
    private bool shouldUpdateUIThisFrame = false;
    private Coroutine GameSaveCoroutine;

    private GameObject topOfList;
    private GameObject bottomOfList;

    public int numberOfBuilding;
    public int maxNumberOfBuilding = 10;

    public static GameManager instance;
    private float secondsLeft;

    public static bool pauseBeacuseADS = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as GameManager;
            DontDestroyOnLoad(gameObject);
            instance.Init();
        }
        else if(instance != null)
        {
            Destroy(gameObject);
        }
    }

    protected void Init()
    {
        managersPopup.OnPopupClose += OnPopupClose;
        upgreadePopup.OnPopupClose += OnPopupClose;
        offlineEarningPopup.OnPopupClose += OnPopupClose;

        numberOfBuilding = 1;
        maxNumberOfBuilding = 10;

        BlockImage.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        managersPopup.OnPopupClose -= OnPopupClose;
        upgreadePopup.OnPopupClose -= OnPopupClose;
        offlineEarningPopup.OnPopupClose -= OnPopupClose;
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");

        StartOrStopGameSaveCoroutine(false);
        PlayerManager.instance.SetGamePaused(true);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("OnPause");
            PlayerManager.instance.OnSaveToDisk();
            StartOrStopGameSaveCoroutine(false);
        }
        else
        {
            if (pauseBeacuseADS == false)
            {
                Debug.Log("OnResume");
                StartOrStopGameSaveCoroutine(true);

                float offlineEarning = PlayerManager.instance.DetermineEarningSinceLastPlay();
                Debug.Log("After determine in pause");
                if (offlineEarning > 0)
                {
                    offlineEarningPopup.Initialize(offlineEarning);
                    offlineEarningPopup.Display();
                }
            }
            else
            {
                pauseBeacuseADS = false;
            }
        }
    }

    private IEnumerator Start()
    {
        FirebaseInit.RegisterEvent("LoadGame", "Value", 1);
        if(FindObjectOfType<FirebaseInit>() != null)
        {
            FindObjectOfType<FirebaseInit>().FetchFirebase();
        }

        //PlayerManager.Create();
        DataManager.Verify();

        //android not turn off screen
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;

        while (GameData.instance == null)
        {
            yield return null;
        }

        while (LocalizationManager.instance == null)
        {
            yield return null;
        }
        //languages
        if (SettingsGame.instance.localizedLanguage <= 0)
        {
            SettingsGame.instance.SetLocalizedLanguage(0);
        }
        LocalizationManager.instance.LoadDatabase();

        MS.instance.StartCoroutine(AdTimerWork());

        Initialize(true);
    }

    private void IsFirstTimeAnimator()
    {
        CanvasGroup mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        mainCanvasGroup.alpha = 0f;
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        CanvasGroup firstTimeCanvasGroup = firstTimeCanvas.GetComponent<CanvasGroup>();
        firstTimeCanvasGroup.alpha = 1f;

        Animator anim = firstTimeCanvas.GetComponent<Animator>();
        anim.SetBool("Show", true);
    }

    public void InitAfterFirstTimeAnimation()
    {
        CanvasGroup mainCanvasGroup = mainCanvas.GetComponent<CanvasGroup>();
        mainCanvasGroup.alpha = 1f;
        mainCanvasGroup.interactable = true;
        mainCanvasGroup.blocksRaycasts = true;

        CanvasGroup firstTimeCanvasGroup = firstTimeCanvas.GetComponent<CanvasGroup>();
        firstTimeCanvasGroup.alpha = 0f;
    }

    public void Initialize(bool enteredGameNow)
    {
        //Show or hide Tutorial
        //Start Animation
        if (SettingsGame.instance.isFirstTimeStatus == true)
        {
            IsFirstTimeAnimator();
            SettingsGame.instance.FirstTimeSetter(false);
        }
        else
        {
            if (dailyRewardsPopup.shouldShowPopup)
            {
                dailyRewardsPopup.ShowPopup();
            }
        }

        float offlineEarning = PlayerManager.instance.DetermineEarningSinceLastPlay();
        if (enteredGameNow == true && offlineEarning > 0)
        {
            offlineEarningPopup.Initialize(offlineEarning);
            offlineEarningPopup.Display();
        }

        if (enteredGameNow == false)
        {
            DestroySlotPanels();
        }

        CreateSlotPanels();

        if(enteredGameNow == true)
        {
            UpdateUI();
            StartOrStopGameSaveCoroutine(true);
        }
    }

    public void TurnSlotPanel(int numberOfBuilding)
    {
        topOfList.GetComponent<Image>().sprite = topListImages[numberOfBuilding - 1];

        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].GetComponent<BuySlotPanel>() != null)
            {
                if(panels[i].GetComponent<BuySlotPanel>().numberOfBuilding != numberOfBuilding)
                {
                    panels[i].GetComponent<Transform>().gameObject.SetActive(false);
                }
                else
                {
                    panels[i].GetComponent<Transform>().gameObject.SetActive(true);
                    panels[i].GetComponent<Image>().sprite = buySlotPanelSprites[numberOfBuilding - 1];
                }
            }
            else if (panels[i].GetComponent<SlotPanel>() != null)
            {
                if (panels[i].GetComponent<SlotPanel>().numberOfBuilding != numberOfBuilding)
                {
                    panels[i].GetComponent<Transform>().gameObject.SetActive(false);
                }
                else
                {
                    panels[i].GetComponent<Transform>().gameObject.SetActive(true);
                    panels[i].GetComponent<SlotPanel>().slotImageBackgroundGO.GetComponent<Image>().sprite = slotPanelSprites[numberOfBuilding - 1];
                }
            }
        }

        bottomOfList.GetComponent<Image>().sprite = bottomListImages[numberOfBuilding - 1];
    }

    public void CreateSlotPanels()
    {
        //TopList
        topOfList = (GameObject)Instantiate(topListGameObject, scrollViewContent);
        topOfList.GetComponent<Image>().sprite = topListImages[numberOfBuilding - 1];

        panels = new MonoBehaviour[GameData.instance.numberOfSlots];
        for (int i = 0; i < panels.Length; i++)
        {
            Slot slot = PlayerManager.instance.GetSlot(panels.Length - 1 - i);

            if (slot.isUnlocked)
            {
                SlotPanel panel = Instantiate(slotPanelPrefab, scrollViewContent);
                panel.Initialize(slot);
                panels[panels.Length - 1 - i] = panel;

                panel.slotImageBackgroundGO.GetComponent<Image>().sprite = slotPanelSprites[numberOfBuilding - 1];

                if (numberOfBuilding != panel.numberOfBuilding)
                {
                    panel.GetComponent<Transform>().gameObject.SetActive(false);
                }
            }
            else
            {
                BuySlotPanel panel = Instantiate(buySlotPanelPrefab, scrollViewContent);
                panel.Initialize(slot.numberOfBuilding, slot.name, slot.costToUnlock, slot.timerToUnlock);
                panels[panels.Length - 1 - i] = panel;

                panel.index = panels.Length - 1 - i;
                
                //panel.slotImageBackgroundGO.GetComponent<Image>().sprite = slotPanelSprites[numberOfBuilding - 1];
                panel.GetComponent<Image>().sprite = buySlotPanelSprites[numberOfBuilding - 1];

                //set delegate
                panel.OnBuyBusinessButtonPressed += (() => { OnBuySlotButtonPressed(panel.index); });

                if (numberOfBuilding != panel.numberOfBuilding)
                {
                    panel.GetComponent<Transform>().gameObject.SetActive(false);
                }
            }
        }

        //BottomList
        bottomOfList = (GameObject)Instantiate(bottomListGameObject, scrollViewContent);
        bottomOfList.GetComponent<Image>().sprite = bottomListImages[numberOfBuilding - 1];
    }

    public void DestroySlotPanels()
    {
        Destroy(topOfList);

        for (int i = 0; i < panels.Length; i++)
        {
            if(panels[i] != null)
            {
                Destroy(panels[i].gameObject);
            }
        }

        Destroy(bottomOfList);
        panels = null;
    }

    //==========================
    public Coroutine InvokeRepeating(Action action, float time, float repeatRate, bool useCachedWaits = true)
    {
        return StartCoroutine(InvokeRepeatingImplementation(action, time, repeatRate, useCachedWaits));
    }

    private IEnumerator InvokeRepeatingImplementation(Action action, float time, float repeatRate, bool useCachedWaits)
    {
        //wait for a given time then indefiently loop - if useCachedYields is true, uses a cached WaitForSeconds, otherwise creates a new one
        yield return (useCachedWaits ? WaitFor.Seconds(time) : new WaitForSeconds(time));
        while (true)
        {
            //invokes the action then waits repeatRate seconds - if useCachedYields is true, uses a cached WaitForSeconds, otherwise creates a new one
            action();
            yield return (useCachedWaits ? WaitFor.Seconds(repeatRate) : new WaitForSeconds(repeatRate));
        }
    }
    //==========================

    private void StartOrStopGameSaveCoroutine(bool shouldStart)
    {
        if (shouldStart)
        {
            if (GameSaveCoroutine == null)
            {
                GameSaveCoroutine = this.InvokeRepeating(() => {
                    PlayerManager.instance.OnSaveToDisk();
                }, 15f, 15f);
            }
            else
            {
                return;
            }
        }
        else
        {
            if (GameSaveCoroutine != null)
            {
                StopCoroutine(GameSaveCoroutine);
                GameSaveCoroutine = null;
                if(GameSaveCoroutine == null)
                {
                    Debug.Log("Now coroutine is null");
                }
            }
            else
            {
                return;
            }
        }
    }

    private void Update()
    {
        if (shouldUpdateUIThisFrame)
        {
            UpdateUI();
            shouldUpdateUIThisFrame = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionsPopup.ShowPopup();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerManager.instance.IncrementCashBy((float)1e14);
            //Debug.Log("OnPause");
            //StartOrStopGameSaveCoroutine(false);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BoosterX2_ActionAfterFinishedAdvert();
            //PlayerManager.instance.DecrementCashBy(PlayerManager.instance.cash);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerManager.instance.DecrementCashBy(PlayerManager.instance.cash);
        }

    }

    public void OnUpdateUI()
    {
        shouldUpdateUIThisFrame = true;
    }

    private void UpdateUI()
    {
        //cashtext
        cashText.text = NumberFormatter.ToString(number: PlayerManager.instance.cash, showDecimalPlaces: true);
        goldText.text = NumberFormatter.ToString(number: PlayerManager.instance.gold, showDecimalPlaces: false, showDollarSign: false);
        adBoosterExtraCashCounter.text = PlayerManager.instance.extraCashBoosterCounter.ToString();

        UpdateStatusExtraCashButton(PlayerManager.instance.extraCashBoosterCounter);
        if (PlayerManager.instance.isBooster)
        {
            if (adBoosterButton.IsInteractable())
                adBoosterButton.interactable = false;

            ulong diff = ((ulong)DateTime.Now.Ticks - PlayerManager.instance.lastBoosterStartDateTime);
            ulong m = diff / TimeSpan.TicksPerMillisecond;

            secondsLeft = (float)(Constant.msToBoosterWait - m) / 1000.0f;

            string r = "";
            //H
            r += ((int)secondsLeft / 3600).ToString() + ":";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;
            //M
            r += ((int)secondsLeft / 60).ToString("00") + ":";
            //S
            r += (secondsLeft % 60).ToString("00") + "";

            adBoosterTimerText.alignment = TextAnchor.MiddleCenter;
            adBoosterTimerText.text = r;
        }
        else
        {
            if (!adBoosterButton.IsInteractable())
                adBoosterButton.interactable = true;

            adBoosterTimerText.fontStyle = FontStyle.Normal;
            adBoosterTimerText.text = "2x\nprofit";
        }
        //update slot panels
        for (int i = 0; i < panels.Length; i++)
        {
            //boosters
            if(panels[i].GetComponent<SlotPanel>() != null)
            {
                Slot slot = PlayerManager.instance.GetSlot(i);
                //show tap to gain money
                if(!slot.isProducing && !PlayerManager._instance.HasBoughtManager(i))
                {
                    panels[i].GetComponent<SlotPanel>().TapToGainMoney(true);
                }
                else
                {
                    panels[i].GetComponent<SlotPanel>().TapToGainMoney(false);
                }

                //show tap to speed up text
                if (slot.isProducing && PlayerManager.instance.HasBoughtManager(slot.index))
                {
                    //panels[i].GetComponent<SlotPanel>().TapToSpeedUpShow(true);
                    panels[i].GetComponent<SlotPanel>().TapToSpeedUpDialogShow(true);
                }
                else
                {
                    //panels[i].GetComponent<SlotPanel>().TapToSpeedUpShow(false);
                    panels[i].GetComponent<SlotPanel>().TapToSpeedUpDialogShow(false);
                }

                //show x2
                if (PlayerManager.instance.isBooster)
                {
                    panels[i].GetComponent<SlotPanel>().SlotIsBoosting(true);
                }
                else
                {
                    panels[i].GetComponent<SlotPanel>().SlotIsBoosting(false);
                }
            }

            if (panels[i].GetComponent<BuySlotPanel>() != null)
            {
                (panels[i] as BuySlotPanel).interactable = 
                    PlayerManager.instance.cash >= PlayerManager.instance.GetSlot(i).costToUnlock && 
                    panels[i-1].GetComponent<SlotPanel>() != null &&
                    PlayerManager.instance.buyButtonParamsToUnlock[i].unlockingIsActive == false;
            }
            else if (panels[i].GetComponent<SlotPanel>() != null)
            {
                (panels[i] as SlotPanel).Refresh();
            }
        }
        //upgreade
        bool upgreadeIsSomething = false;
        //check 10 (max number of upgrades)
        for (int i = numberOfBuilding * 10 - 10; i < 10; i++)
        {
            if (!PlayerManager.instance.HasBoughtUpgreade(i))
            {
                upgreadeIsSomething = true;
                if (PlayerManager.instance.cash >= PlayerManager.instance.GetUpgreadeCost(i) &&
                    panels[i].GetComponent<SlotPanel>() != null)
                {
                    upgreadesNotification.alpha = 1f;
                    upgradesBlurBackground.alpha = 1f;
                }
                else
                {
                    upgreadesNotification.alpha = 0f;
                    upgradesBlurBackground.alpha = 0f;
                }
                break;
            }

            //list is empty
            if(upgreadeIsSomething == false)
            {
                upgreadesNotification.alpha = 0f;
                upgradesBlurBackground.alpha = 0f;
            }
        }
        if (upgreadePopup.GetStatus())
        {
            for (int upgreadeIndex = 0; upgreadeIndex < upgreadePopup.panels.Count; upgreadeIndex++)
            {
                if (upgreadePopup.panels[upgreadeIndex].cost <= PlayerManager.instance.cash)
                {
                    upgreadePopup.panels[upgreadeIndex].RefreshBuyButtonStatus(true);
                }
                else
                {
                    upgreadePopup.panels[upgreadeIndex].RefreshBuyButtonStatus(false);
                }
            }
        }
        //manager
        bool managerIsSomething = false;
        //check 20 (max number of upgrades)
        for (int i = numberOfBuilding * 20 - 20; i < 20; i++)
        {
            if (!PlayerManager.instance.HasBoughtManager(i))
            {
                managerIsSomething = true;
                float tmp = PlayerManager.instance.GetManagerCost(i);
                if (PlayerManager.instance.cash >= PlayerManager.instance.GetManagerCost(i) &&
                    panels[i].GetComponent<SlotPanel>() != null)
                {
                    managersNotification.alpha = 1f;
                    managersBlurBackground.alpha = 1f;
                }
                else
                {
                    managersNotification.alpha = 0f;
                    managersBlurBackground.alpha = 0f;
                }
                break;
            }

            //list is empty
            if(managerIsSomething == false)
            {
                managersNotification.alpha = 0f;
                managersBlurBackground.alpha = 0f;
            }
        }
        if (managersPopup.GetStatus())
        {
            for (int managerIndex = 0; managerIndex < managersPopup.panels.Count; managerIndex++)
            {
                if (managersPopup.panels[managerIndex].cost <= PlayerManager.instance.cash)
                {
                    managersPopup.panels[managerIndex].RefreshBuyButtonStatus(true);
                }
                else
                {
                    managersPopup.panels[managerIndex].RefreshBuyButtonStatus(false);
                }
            }
        }
        if (PlayerManager.instance.shouldConsiderContract)
        {
            officeNotification.alpha = 1f;
            officeBlurBackground.alpha = 1f;
        }
        else
        {
            officeNotification.alpha = 0f;
            officeBlurBackground.alpha = 0f;
        }

        //contract Panel
        if (officePopup != null && officePopup.isShow)
        {
            contract.RefreshContractPanel();
        }
        //map buildings
        if (mapPopup.GetStatus())
        {
            for (int i = 0; i < mapPopup.buildingMap.Length; i++)
            {
                if (PlayerManager.instance.cash >= PlayerManager.instance.GetBuildingMapCost(i))
                {
                    mapPopup.buildingMap[i].RefreshBuyButton(true);
                }
                else
                {
                    mapPopup.buildingMap[i].RefreshBuyButton(false);
                }
            }
        }
        //upgrade each floor panel
        if (upgradeEachFloorPopup.isVisible)
        {
            upgradeEachFloorPopup.Refresh();
        }
    }

    #region Callbacks

    private void OnBuySlotButtonPressed(int slotIndex)
    {
        BuySlotPanel buySlotPanel = panels[slotIndex].GetComponent<BuySlotPanel>();
        Slot slot = PlayerManager.instance.GetSlot(slotIndex);

        if (!buySlotPanel.nowIsUnlocking)
        {
            if (PlayerManager.instance.cash >= slot.costToUnlock)
            {
                PlayerManager.instance.DecrementCashBy(slot.costToUnlock);
                buySlotPanel.ShowTimerToUnlockUI();
            }
        }
    }

    public void OnBuySlotButtonUnlockPressed(int slotIndex)
    {
        Slot slot = PlayerManager.instance.GetSlot(slotIndex);

        SlotPanel panel = Instantiate(slotPanelPrefab, scrollViewContent);
        panel.transform.SetSiblingIndex(panels.Length - slotIndex);
        panel.Initialize(slot);
        
        Destroy(panels[slotIndex].gameObject);
        panels[slotIndex] = panel;
        
        slot.SetUnlocked();
        
        UpdateUI();
    }

    public void UpdateMainViewWhenBannerIsActive()
    {
        GameObject go = Instantiate(whenBannerActiveImagePrefab, scrollViewContent);
        go.transform.SetAsLastSibling();

        UpdateUI();
    }

    private void OnPopupClose()
    {
        mainCanvas.GetComponent<CanvasGroup>().interactable = true;
        mainCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;

        shouldUpdateUIThisFrame = true;
    }

    public void OnShowManagersPopupButtonPressed()
    {
        CanvasGroup mainCanvasGroup = managersPopup.GetComponent<CanvasGroup>();
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        managersPopup.Display(numberOfBuilding);
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
    }

    public void OnShowOfficePopupButtonPressed()
    {
        officePopup.Display();
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
    }

    public void OnShowUpgreadePopupButtonPressed()
    {
        CanvasGroup mainCanvasGroup = upgreadePopup.GetComponent<CanvasGroup>();
        mainCanvasGroup.interactable = false;
        mainCanvasGroup.blocksRaycasts = false;

        upgreadePopup.Display(numberOfBuilding);
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
    }

    public void OnADSBoosterX2ButtonPressed()
    {
        UnityADSManager.instance.ShowRewardedVideo(UnityADSManager.BoosterType.x2);
    }

    public void BoosterX2_ActionAfterFinishedAdvert()
    {
        adBoosterButton.interactable = false;

        PlayerManager.instance.AdBooster(true);
        PlayerManager.instance.lastBoosterStartDateTime = (ulong)DateTime.Now.Ticks;

        shouldUpdateUIThisFrame = true;
    }

    public void UpdateStatusExtraCashButton(int valueOfExtraCashCounter)
    {
        if(valueOfExtraCashCounter > 0)
        {
            adBoosterExtraCashCounterPopup.SetActive(true);
            IAPadBoosterExtraCashCounterPopup.SetActive(false);
        }
        else
        {
            adBoosterExtraCashCounterPopup.SetActive(false);
            IAPadBoosterExtraCashCounterPopup.SetActive(true);
        }
    }

    public void OnADSBoosterExtraCashButtonPressed()
    {
        if (FindObjectOfType<ExtraCashPopup>() != null)
        {
            FindObjectOfType<ExtraCashPopup>().WhenAnimationStart_Caller();
        }

        if (PlayerManager.instance.extraCashBoosterCounter > 0)
        {
            PlayerManager.instance.extraCashBoosterCounter--;
            ExtraCash();

            UpdateStatusExtraCashButton(PlayerManager.instance.extraCashBoosterCounter);

            shouldUpdateUIThisFrame = true;
        }
    }

    public float CountExtraCash()
    {
        float cash = 0f;

        for (int i = 0; i < GameData.instance.numberOfSlots; i++)
        {
            Slot slot = PlayerManager.instance.GetSlot(i);
            if (slot.isUnlocked == false)
            {
                slot = PlayerManager.instance.GetSlot(i - 1);
                cash = slot.profit * 2;

                break;
            }
            else if (i == GameData.instance.numberOfSlots - 1)
            {
                cash = slot.profit;

                break;
            }
        }

        return cash;
    }

    private void ExtraCash()
    {
        float value = CountExtraCash();

        if (value > 0)
        {
            extraCashText.text = "+" + NumberFormatter.ToString(number: value, showDecimalPlaces: true);
            PlayerManager.instance.IncrementCashBy(value);

            extraCashGO.GetComponent<Animator>().SetTrigger("Show");
        }
    }

    public void BoosterExtraCash_ActionAfterFinishedAdvert()
    {
        PlayerManager.instance.extraCashBoosterCounter++;

        adBoosterExtraCashCounterPopup.GetComponent<Animator>().SetTrigger("Show");
        adBoosterExtraCashCounter.text = PlayerManager.instance.extraCashBoosterCounter.ToString();
    }

    public IEnumerator AdTimerWork()
    {
        while(true)
        {
            ulong diff = ((ulong)DateTime.Now.Ticks - PlayerManager.instance.lastBoosterStartDateTime);
            ulong m = diff / TimeSpan.TicksPerMillisecond;

            secondsLeft = (float)(Constant.msToBoosterWait - m) / 1000.0f;

            if (secondsLeft <= 0)
            {
                PlayerManager.instance.AdBooster(false);
                PlayerManager.instance.isBooster = false;
                shouldUpdateUIThisFrame = true;
            }
            shouldUpdateUIThisFrame = true;
            yield return null;
        }
    }

    public void OnADSDoubleOfflineEarningButtonPressed()
    {
        UnityADSManager.instance.ShowRewardedVideo(UnityADSManager.BoosterType.doubleOfflineEarning);
    }

    public void DoubleOfflineEarning_ActionAfterFinishedAdvert()
    {
        OfflineEarningDoubleAfterADSPopup offlineEarningAfterAdsPopup = FindObjectOfType<OfflineEarningDoubleAfterADSPopup>();
        OfflineEarning offlineEarningPopup = FindObjectOfType<OfflineEarning>();

        offlineEarningAfterAdsPopup.Show(offlineEarningPopup.GetCalculatedOfflineEarningValue());
    }

    public void RandomBonus_ActionAfterFinishedAdvert()
    {
        RandomBonus randomBonus = FindObjectOfType<RandomBonus>();
        randomBonus.AddCashAfterAds();
    }

    public void CutTimerToUnlockSlot_ActionAfterFinishedAdvert(int index)
    {
        BuySlotPanel buySlotPanel = panels[index].GetComponent<BuySlotPanel>();
        buySlotPanel.afterBooster = true;
    }

    #endregion


    public void BlockImageTurn(bool value)
    {
        if (value)
        {
            BlockImage.gameObject.SetActive(true);
        }
        else
        {
            BlockImage.gameObject.SetActive(false);
        }
    }
}