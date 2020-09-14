using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfficePopup : MonoBehaviour
{
    public delegate void EventHandler();
    public EventHandler OnPopupClose;

    public Text title;
    public Button contractButton;
    [SerializeField]
    private Text headingText;
    [SerializeField]
    private Text numberOfContracts_text;
    [SerializeField]
    private Text numberOfContracts_Value;
    [SerializeField]
    private Text youKeep_text;
    [SerializeField]
    private Text otherBuildings_text;
    [SerializeField]
    private Text goldCoins_text;
    [SerializeField]
    private Text profitText_leftSide;
    [SerializeField]
    private Text valueText_leftSide;
    [SerializeField]
    private Text profitText_rightSide;
    [SerializeField]
    private Text valueText_rightSide;
    [SerializeField]
    private Text levelPriceText;
    [SerializeField]
    private Text contractButtonText;

    public bool isShow { get; set; }
    private Animator animator;

    public AfterContractPopup popupAfterContract;

    public void Start()
    {
        animator = GetComponent<Animator>();
        isShow = false;
    }

    public void Display()
    {
        title.text = LocalizationManager.instance.StringForKey("OfficePanel_Title");
        headingText.text = LocalizationManager.instance.StringForKey("OfficePanel_Heading");
        numberOfContracts_text.text = LocalizationManager.instance.StringForKey("OfficePanel_ContractNumbers");

        int numberOfContracts = PlayerManager.instance.level - 1;
        numberOfContracts_Value.text = numberOfContracts.ToString();

        youKeep_text.text = LocalizationManager.instance.StringForKey("OfficePanel_YouKeep");
        otherBuildings_text.text = LocalizationManager.instance.StringForKey("OfficePanel_OtherBuildings");
        goldCoins_text.text = LocalizationManager.instance.StringForKey("OfficePanel_GoldCoins");
        profitText_leftSide.text = NumberFormatter.ToString(Mathf.Pow(Constant.percentageContractValue, PlayerManager.instance.level-1) * 100, false, false, false) + "%";
        valueText_leftSide.text = LocalizationManager.instance.StringForKey("OfficePanel_ValueText");
        profitText_rightSide.text = NumberFormatter.ToString(Mathf.Pow(Constant.percentageContractValue, PlayerManager.instance.level) * 100, false, false, false) + "%";
        valueText_rightSide.text = LocalizationManager.instance.StringForKey("OfficePanel_ValueText");
        levelPriceText.text = "$" + NumberFormatter.ToString(PlayerManager.instance.contractPrice, true, false);
        contractButtonText.text = LocalizationManager.instance.StringForKey("blockyButtonText");

        if (PlayerPrefs.GetInt("OfficePopup") != 1)
        {
            FindObjectOfType<TutorialManager>().PlayTutorialStep(4);
            PlayerPrefs.SetInt("OfficePopup", 1);
        }

        animator.SetTrigger("Show");
        isShow = true;
    }

    private void Hide()
    {
        animator.SetTrigger("Hide");
        isShow = false;
    }

    public void ShowResetAnim()
    {
        animator.SetTrigger("ShowContractAnim");
        isShow = false;
    }

    public void ShowPanelAfterContract_Caller()
    {
        popupAfterContract.ShowPopup();
        isShow = false;
    }

    public void ContractFunctionBackend_Caller()
    {
        ContractPanel contractPanel = gameObject.GetComponentInChildren<ContractPanel>();
        contractPanel.ContractFunctionBackend();
    }

    public void ExitButtonPressed()
    {
        Hide();
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
        if (OnPopupClose != null)
        {
            OnPopupClose();
        }
    }
}
