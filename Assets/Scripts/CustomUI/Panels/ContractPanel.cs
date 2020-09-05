using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractPanel : MonoBehaviour
{
    public OfficePopup office;
    public ConfirmationPopup confirmationPopup;

    public void RefreshContractPanel()
    {
        if (PlayerManager.instance.shouldConsiderContract)
        {
            office.contractButton.image.color = GameColors.availableColorForButtons;
        }
        else
        {
            office.contractButton.image.color = GameColors.disableColorForButtons;
        }
    }

    public void OnContractButtonPressed()
    {
        if (PlayerManager.instance.shouldConsiderContract)
        {
            ConfirmationPopup.afterConfirmationDelegate = DelegateAfterConfirmation_ContractPanel;
            StartCoroutine(confirmationPopup.CallConfirmationPanel("ConfirmationPanel_ContractText", 1, PlayerManager.instance.XP));
        }
    }

    public void DelegateAfterConfirmation_ContractPanel()
    {
        if (PlayerManager.instance.shouldConsiderContract)
        {
            OfficePopup officePopup = gameObject.GetComponentInParent<OfficePopup>();
            officePopup.ShowResetAnim();
        }
    }

    public void ContractFunctionBackend()
    {
        //hide officePanel
        StartCoroutine(PlayerManager.instance.Contract());
        GameManager.instance.DestroySlotPanels();
        GameManager.instance.CreateSlotPanels();

        int size = PlayerManager.instance.buyButtonParamsToUnlock.Length;
        for (int i = 0; i < size; i++)
        {
            PlayerManager.instance.buyButtonParamsToUnlock[i].unlockingIsActive = false;
        }

        //reset camera
        GameManager.instance.scrollView.verticalNormalizedPosition = 0f;
    }
}
