using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPopup : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text[] buttons_text;
    //private Text[] slots_goldText;
    [SerializeField]
    private Text removeAdsText;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    public void BackButtonPressed()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowPopup()
    {
        title.text = LocalizationManager.instance.StringForKey("Shop_Title");
        for (int i = 0; i < buttons_text.Length; i++)
        {
            buttons_text[i].text = LocalizationManager.instance.StringForKey("ButtonsShop_Text");
        }

        /*for (int i = 0; i < slots_goldText.Length; i++)
        {
            slots_goldText[i].text = LocalizationManager.instance.StringForKey("SlotShop_GoldText");
        }*/
        //removeAdsText.text = LocalizationManager.instance.StringForKey("RemoveAdsText");

        animator.SetTrigger("Show");
    }
}
