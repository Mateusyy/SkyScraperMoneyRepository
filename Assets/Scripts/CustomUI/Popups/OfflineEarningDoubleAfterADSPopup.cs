using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineEarningDoubleAfterADSPopup : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private Text priceText;

    public OfflineEarning offlineEarning;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(float offlineEarningValue)
    {
        SetParam(priceText, offlineEarningValue);
        anim.SetTrigger("Show");
    }

    private void SetParam(Text priceText, float offlineEarning)
    {
        if (offlineEarning * 2 < 1000f)
            priceText.text = NumberFormatter.ToString(offlineEarning * 2, true, true, false);
        else
            priceText.text = NumberFormatter.ToString(offlineEarning * 2, false, true, false);
    }

    public void Func_AddOfflineEarningValue()
    {
        PlayerManager.instance.IncrementCashBy(offlineEarning.GetCalculatedOfflineEarningValue() * 2);
    }
}
