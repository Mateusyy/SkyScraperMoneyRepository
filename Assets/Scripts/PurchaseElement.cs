using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class PurchaseElement : MonoBehaviour
{
    public void PurchaseExtraCash()
    {
        if(PlayerManager.instance.gold >= 30)
        {
            PlayerManager.instance.DecrementGoldBy(30);
            StartCoroutine(UpdateStatusOfButton());
        }
        else
        {
            FindObjectOfType<ShopPopup>().ShowPopup();
        }
    }

    public void OnPurchaseComplete1000Gold(Product product)
    {
        StartCoroutine(UpdateStatusOfButton1000Gold());
    }

    public void OnPurchaseComplete600Gold(Product product)
    {
        StartCoroutine(UpdateStatusOfButton600Gold());
    }

    public void OnPurchaseComplete200Gold(Product product)
    {
        StartCoroutine(UpdateStatusOfButton200Gold());
    }

    public void OnPurchaseComplete90Gold(Product product)
    {
        StartCoroutine(UpdateStatusOfButton90Gold());
    }

    public void OnPurchaseComplete40Gold(Product product)
    {
        StartCoroutine(UpdateStatusOfButton40Gold());
    }

    public void OnPurchaseCompleteRemoveAds(Product product)
    {
        StartCoroutine(UpdateStatusOfButtonRemoveAds());
    }

    public void OnPurchaseFailure(Product product, PurchaseFailureReason purchaseFailureReason)
    {
        Debug.Log("Purchase product: " + product.definition.id + " was failure due to reason: " + purchaseFailureReason);
    }

    private IEnumerator UpdateStatusOfButton()
    {
        yield return new WaitForEndOfFrame();
        GameManager.instance.UpdateStatusExtraCashButton(PlayerManager.instance.extraCashBoosterCounter + 1);
        GameManager.instance.BoosterExtraCash_ActionAfterFinishedAdvert();
    }

    private IEnumerator UpdateStatusOfButton1000Gold()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.instance.IncrementGoldBy(1000);
    }

    private IEnumerator UpdateStatusOfButton600Gold()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.instance.IncrementGoldBy(600);
    }

    private IEnumerator UpdateStatusOfButton200Gold()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.instance.IncrementGoldBy(200);
    }

    private IEnumerator UpdateStatusOfButton90Gold()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.instance.IncrementGoldBy(90);
    }

    private IEnumerator UpdateStatusOfButton40Gold()
    {
        yield return new WaitForEndOfFrame();
        PlayerManager.instance.IncrementGoldBy(40);
    }

    private IEnumerator UpdateStatusOfButtonRemoveAds()
    {
        yield return new WaitForEndOfFrame();
        //PlayerManager.instance.IncrementGoldBy(40);
        //TODO: remove ads
    }
}
