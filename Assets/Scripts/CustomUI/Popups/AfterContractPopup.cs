using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AfterContractPopup : MonoBehaviour
{
    private Animator anim;
    private int managerIndex;

    //[SerializeField]
    //private Image button_1_image;
    [SerializeField]
    private Text button_1_text;

    //[SerializeField]
    //private Image button_2_image;
    [SerializeField]
    private Text button_2_text;

    [SerializeField]
    private Image button_3_image;
    [SerializeField]
    private Text button_3_text;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void ShowPopup()
    {
        LoadPopupData();
        anim.SetTrigger("Show");
    }

    public void HidePopup()
    {
        anim.SetTrigger("Hide");
    }

    private void LoadPopupData()
    {
        //slot 1
        button_1_text.text = LocalizationManager.instance.StringForKey("button1text");

        //slot 2
        button_2_text.text = "+" + (PlayerManager.instance.level-1) + " " + LocalizationManager.instance.StringForKey("button2text");

        //slot 3
        managerIndex = Random.Range(0, 9);
        ManagerData managerData = GameData.instance.GetDataForManager(managerIndex);
        button_3_image.sprite = managerData.image;
        button_3_text.text = LocalizationManager.instance.StringForKey("button3text_1") + LocalizationManager.instance.StringForKey(managerData.slot.ToString().ToUpper()) + LocalizationManager.instance.StringForKey("button3text_2");
    }

    public void OnGetProfit(int index)
    {
        //index
        switch (index)
        {
            case 0:
                PlayerManager.instance.maxTimeOfflineEarning += 3600f;
                break;
            case 1:
                PlayerManager.instance.IncrementGoldBy(PlayerManager.instance.level - 1);
                break;
            case 2:
                PlayerManager.instance.BoughtManager(managerIndex);
                break;
        }

        HidePopup();
    }
}
