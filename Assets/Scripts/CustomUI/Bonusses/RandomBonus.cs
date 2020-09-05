using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RandomBonus : MonoBehaviour
{
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private Text bonusCashText;
    
    private Animator anim;
    private bool leftSide;
    private bool rightSide;
    private bool working;
    private bool canBeClickable;

    private float cash;
    private RandomBonusPopup randomBonusPopup;

    void Awake()
    {
        leftSide = true;
        rightSide = false;
        working = false;

        if(canvas == null || bonusCashText == null)
        {
            throw new System.Exception("Some references is empty!");
        }

        anim = GetComponent<Animator>();
        randomBonusPopup = FindObjectOfType<RandomBonusPopup>();

    }

    void Update()
    {
        Working();
    }

    private void Working()
    {
        if(working == false)
        {
            float y = UnityEngine.Random.Range(-7270, -2000);
            gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(GetComponent<RectTransform>().anchoredPosition.x, y, GetComponent<RectTransform>().anchoredPosition.x);
            canBeClickable = true;
            working = true;
        }

        if (working == true)
        {
            if (leftSide == true && rightSide == false)
            {
                if(GetComponent<RectTransform>().anchoredPosition.x < 2000f)
                {
                    GoToRight(4f);
                }
                else
                {
                    leftSide = false;
                    rightSide = true;
                    working = false;

                    if(!canBeClickable)
                        anim.SetTrigger("RestartAnimation");
                }
            }
            else if (rightSide == true && leftSide == false)
            {
                if (GetComponent<RectTransform>().anchoredPosition.x > -2000f)
                {
                    GoToLeft(4f);
                }
                else
                {
                    leftSide = true;
                    rightSide = false;
                    working = false;

                    if (!canBeClickable)
                        anim.SetTrigger("RestartAnimation");
                }
            }
            else
            {
                throw new System.Exception("Unavailable status for random bonus!");
            }
        }
    }

    private void GoToLeft(float speed)
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void GoToRight(float speed)
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void ClickedAction()
    {
        if (canBeClickable)
        {
            int versionOfBonus = UnityEngine.Random.Range(0, 10);

            if (versionOfBonus < 6)    //ads bonus
            {
                cash = 100f;
                float value = GameManager.instance.CountExtraCash() / 2;
                if (value > 100f)
                {
                    cash = value;
                }
                RandomBonusPopup.afterConfirmationDelegate = DelegateAfterConfirmation_RandomBonusWithAds;
                StartCoroutine(randomBonusPopup.CallConfirmationPanel("ConfirmationPanel_RandomBonus", cash));
                anim.SetTrigger("ShowWithAds");
            }
            else //standard bonus
            {
                cash = 100f;
                if (GameManager.instance.CountExtraCash() > 100f)
                {
                    cash = GameManager.instance.CountExtraCash() / 5;
                }

                bonusCashText.text = NumberFormatter.ToString(cash, true, true);
                PlayerManager.instance.IncrementCashBy(cash);
                anim.SetTrigger("Show");
            }

            canBeClickable = false;
        }
    }

    public void DelegateAfterConfirmation_RandomBonusWithAds()
    {
        UnityADSManager.instance.ShowRewardedVideo(UnityADSManager.BoosterType.randomBonus);
    }

    public void AddCashAfterAds()
    {
        PlayerManager.instance.IncrementCashBy(cash);
    }
}
