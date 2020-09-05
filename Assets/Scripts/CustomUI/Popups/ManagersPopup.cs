using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Collections;
using UnityEngine.UI;

public class ManagersPopup : MonoBehaviour
{
    public delegate void EventHandler();
    public EventHandler OnPopupClose;

    [SerializeField]
    RectTransform scrollViewContent;
    [SerializeField]
    ManagerPanel managerPanelPrefab;

    [SerializeField]
    private Text title;
    [SerializeField]
    private Text title_desc;

    public List<ManagerPanel> panels;

    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Display(int numberOfBuilding)
    {
        title.text = LocalizationManager.instance.StringForKey("ManagersPanel_Title");
        title_desc.text = LocalizationManager.instance.StringForKey("ManagersPanel_TitleDesc");

        if (panels == null)
        {
            panels = new List<ManagerPanel>();
        }
        for (int managerIndex = 0; managerIndex < GameData.instance.numberOfManagers; managerIndex++)
        {
            if (!PlayerManager.instance.HasBoughtManager(managerIndex))
            {
                ManagerPanel panel = Instantiate(managerPanelPrefab, scrollViewContent);
                panel.Initialize(managerIndex);
                panels.Add(panel);

                if (numberOfBuilding != panel.numberOfBuilding)
                {
                    panel.GetComponent<Transform>().gameObject.SetActive(false);
                }
            }
        }

        animator.SetTrigger("Show");
    }

    public void Hide()
    {
        Assert.IsNotNull(panels);

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