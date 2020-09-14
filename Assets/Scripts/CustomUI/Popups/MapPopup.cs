using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPopup : MonoBehaviour
{
    [SerializeField]
    private Text title;
    private Animator animator;
    public BuildingMap[] buildingMap;
    public ConfirmationPopup confirmationPopup;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void LoadInformationsAboutBuildings()
    {
        title.text = LocalizationManager.instance.StringForKey("MapPopup_Title");

        for (int i = 0; i < buildingMap.Length; i++)
        {
            //buildingMap[i].Initialize(i);
            buildingMap[i].RefreshUI();
            buildingMap[i].SetBuildingTitleTextValue();
            buildingMap[i].SetCostOfBuilding();
            buildingMap[i].SetNumberOfFloorsAndCashPerSecondTextValue(i);
        }
    }

    public void HidePopup()
    {
        animator.SetTrigger("Hide");
    }

    public void ShowPopup()
    {
        if (PlayerPrefs.GetInt("MapPopup") != 1)
        {
            FindObjectOfType<TutorialManager>().PlayTutorialStep(5);
            PlayerPrefs.SetInt("MapPopup", 1);
        }

        LoadInformationsAboutBuildings();
        animator.SetTrigger("Show");
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

    private int indexToChangeBuilding = 0;
    public void TurnNewBuilding(int index)
    {
        indexToChangeBuilding = index;

        if(GameManager.instance.numberOfBuilding != index)
        {
            if (PlayerManager.instance.HasBoughtBuildingMap(index-1))
            {
                ConfirmationPopup.afterConfirmationDelegate = DelegateAfterConfirmation_ChangeBuilding;
                StartCoroutine(confirmationPopup.CallConfirmationPanel("ConfirmationPanel_ChangeBuilding"));
            }
        }
        else
        {
            HidePopup();
            BlockyImage();
        }
    }

    public void DelegateAfterConfirmation_ChangeBuilding()
    {
        GameManager.instance.TurnSlotPanel(indexToChangeBuilding);
        GameManager.instance.numberOfBuilding = indexToChangeBuilding;
        HidePopup();
        BlockyImage();
    }

    private void BlockyImage()
    {
        //delete when second building is available
        if (GameManager.instance.numberOfBuilding > 2)
        {
            GameManager.instance.BlockImageTurn(true);
        }
        else
        {
            GameManager.instance.BlockImageTurn(false);
        }
    }
}
