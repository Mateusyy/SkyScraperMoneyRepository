using Firebase.Database;
using Proyecto26;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopup : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Text backButton_text;
    [SerializeField]
    private Text _exitButton_text;
    [SerializeField]
    private Text _options_title;
    [SerializeField]
    private Text selectLanguage_text;
    [SerializeField]
    private Text sounds_text;
    [SerializeField]
    private Text notification_text;

    public void Start()
    {
        animator = GetComponent<Animator>();
        RefreshOptionPanel();
    }

    public void SelectLanguagePressed(int index)
    {
        SettingsGame.instance.SetLocalizedLanguage(index);
        LocalizationManager.instance.LoadDatabase();
        RefreshSlotsLanguage();
        RefreshOptionPanel();
        BackButtonPressed();
    }

    private void RefreshSlotsLanguage()
    {
        for (int i = 0; i < GameManager.instance.panels.Length; i++)
        {
            if (GameManager.instance.panels[i].GetComponent<SlotPanel>() != null)
            {
                (GameManager.instance.panels[i] as SlotPanel).RefreshLanguage();
            }
            else if (GameManager.instance.panels[i].GetComponent<BuySlotPanel>() != null)
            {
                (GameManager.instance.panels[i] as BuySlotPanel).RefreshLanguageBuySlotPanel();
            }
        }
    }

    private void RefreshOptionPanel()
    {
        _options_title.text = LocalizationManager.instance.StringForKey("OptionsPanel_title");
        backButton_text.text = LocalizationManager.instance.StringForKey("OptionsPanel_backButtonText");
        _exitButton_text.text = LocalizationManager.instance.StringForKey("OptionsPanel_exitButtonText");
        selectLanguage_text.text = LocalizationManager.instance.StringForKey("OptionsPanel_selectLanguageText");
        sounds_text.text = LocalizationManager.instance.StringForKey("OptionsPanel_soundsText");
        notification_text.text = LocalizationManager.instance.StringForKey("OptionsPanel_notificationText");
    }

    public void ExitButtonPressed()
    {
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
        Application.Quit();
    }

    public void BackButtonPressed()
    {
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
        animator.SetTrigger("Hide");
    }

    public void ShowPopup()
    {
        FindObjectOfType<OpenCloseAudioSource>().PlaySound();
        animator.SetTrigger("Show");
    }

    public void AddExtraMoney()
    {
        PlayerManager.instance.IncrementCashBy((float)1e9);
    }
}