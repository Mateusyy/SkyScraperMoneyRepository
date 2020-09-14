using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UpgreadePopup : MonoBehaviour
{
    public delegate void EventHandler();
    public EventHandler OnPopupClose;

    [SerializeField]
    RectTransform scrollViewContent;
    [SerializeField]
    UpgreadePanel upgreadePanelPrefab;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text title_desc;

    public List<UpgreadePanel> panels;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Display(int numberOfBuilding)
    {
        title.text = LocalizationManager.instance.StringForKey("UpgradePanel_title");
        title_desc.text = LocalizationManager.instance.StringForKey("UpgradePanel_titleDesc");

        if (panels == null)
        {
            panels = new List<UpgreadePanel>();
        }
        for (int upgreadeIndex = 0; upgreadeIndex < GameData.instance.numberOfUpgreades; upgreadeIndex++)
        {
            if (!PlayerManager.instance.HasBoughtUpgreade(upgreadeIndex))
            {
                UpgreadePanel panel = Instantiate(upgreadePanelPrefab, scrollViewContent);
                panel.Initialize(upgreadeIndex);
                panels.Add(panel);

                if (numberOfBuilding != panel.numberOfBuilding)
                {
                    panel.GetComponent<Transform>().gameObject.SetActive(false);
                }
            }
        }

        if (PlayerPrefs.GetInt("UpgradesPopup") != 1)
        {
            FindObjectOfType<TutorialManager>().PlayTutorialStep(6);
            PlayerPrefs.SetInt("UpgradesPopup", 1);
        }
        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        Assert.IsNotNull(panels);

        //CanvasGroup mainCanvasGroup = GetComponent<CanvasGroup>();
        //mainCanvasGroup.interactable = false;
        //mainCanvasGroup.blocksRaycasts = false;
        //mainCanvasGroup.alpha = 0f;
        animator.SetTrigger("Hide");

        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i] != null)
            {
                Destroy(panels[i].gameObject);
            }
        }
        panels.Clear();
    }

    public bool GetStatus()
    {
        CanvasGroup mainCanvasGroup = GetComponent<CanvasGroup>();

        if (mainCanvasGroup.interactable == true && mainCanvasGroup.blocksRaycasts == true && mainCanvasGroup.alpha == 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
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