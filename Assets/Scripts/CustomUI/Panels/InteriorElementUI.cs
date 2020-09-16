using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteriorElementUI : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private CanvasGroup panel;

    [SerializeField]
    private Sprite hammerIcon;

    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text firstTextInfo;
    [SerializeField]
    private Text secondTextInfo;

    [SerializeField]
    private Button actionButton;
    private bool isActive = false;

    private InteriorElement interiorElement;
    private Slot slot;
    private int index;
    private bool playAvailableAnimation = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        interiorElement = transform.parent.GetComponent<InteriorElement>();
        actionButton.onClick.AddListener(() => { Debug.Log("ActionButtonCall"); });

        actionButton.onClick.AddListener(() =>
        {
            OnMainButtonPressed(interiorElement, index);
        });
    }

    public void Initialize(Slot slot, int i)
    {
        this.slot = slot;
        this.index = i;
    }

    private void Update()
    {
        if (isActive)
        {
            if (slot.GetInformationsAboutObjectToUnlock(index))
            {
                //BOUGHT
                TurnOffUiElements();
            }

            if (interiorElement.status != InteriorElementStatus.BOUGHT && slot.level < slot.GetMilestoneLevelTarget(index))
            {
                //NOT AVAILABLE
                icon.sprite = hammerIcon;
                firstTextInfo.text = slot.GetMilestoneLevelTarget(index).ToString() + " lvl";
                secondTextInfo.text = LocalizationManager.instance.StringForKey("to_unlock_text");

                TurnOnUiElements(false);
            }

            if (interiorElement.status != InteriorElementStatus.BOUGHT && slot.level >= slot.GetMilestoneLevelTarget(index))
            {
                //AVAILABLE
                if (PlayerManager.instance.cash >= interiorElement.price)
                {
                    anim.ResetTrigger("PlayIdleAnim");
                    anim.SetTrigger("PlayAvailableAnim");
                    playAvailableAnimation = true;
                    TurnOnUiElements(true);
                }
                else
                {
                    if (playAvailableAnimation)
                    {
                        anim.ResetTrigger("PlayAvailableAnim");
                        anim.SetTrigger("PlayIdleAnim");
                        playAvailableAnimation = false;
                    }
                    TurnOnUiElements(false);
                }

                icon.sprite = hammerIcon;
                firstTextInfo.text = NumberFormatter.ToString(interiorElement.price, false, true, false);
                secondTextInfo.text = LocalizationManager.instance.StringForKey("to_buy_text");

            }
        }
    }

    public void TurnOffUiElements()
    {
        isActive = false;

        panel.alpha = 0f;
        panel.interactable = false;
        panel.blocksRaycasts = false;
    }

    public void TurnOnUiElements(bool clickable = false)
    {
        isActive = true;

        panel.alpha = 1f;
        if (clickable)
        {
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }
        else
        {
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }
    }

    private void OnMainButtonPressed(InteriorElement interiorElement, int index)
    {
        float value = slot.GetMilestoneValue(index);

        if (interiorElement.symbol == null)
            FindObjectOfType<BigUpgradeFloorsForAnimationPopup>().Show(interiorElement.name, value.ToString() + "x", interiorElement.GetComponent<SpriteRenderer>().sprite);
        else
            FindObjectOfType<BigUpgradeFloorsForAnimationPopup>().Show(interiorElement.name, value.ToString() + "x", interiorElement.symbol);

        PlayerManager.instance.DecrementCashBy(interiorElement.price);
        interiorElement.status = InteriorElementStatus.BOUGHT;
        slot.SetUnlockedObject(index);
        interiorElement.TurnOnElement();

        slot.UpdateLevelBy(0);  //only to get multiplier
        GameManager.instance.OnUpdateUI();
        actionButton.interactable = false;
    }
}
