using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBuilding : MonoBehaviour
{
    [SerializeField]
    private Button leftArrow;
    [SerializeField]
    private Button rightArrow;

    [SerializeField]
    private Text num_building;

    private IEnumerator Start()
    {
        while (GameManager.instance == null)
        {
            yield return null;
        }

        SetNumberOfBuildingText(GameManager.instance.numberOfBuilding);
        SetArrowActiveOrDisable(GameManager.instance.numberOfBuilding);

        SetVisible();
    }

    public void SetVisible()
    {
        if( gameObject.GetComponent<CanvasGroup>().alpha == 1.0f &&
            gameObject.GetComponent<CanvasGroup>().interactable == true &&
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts == true)
        {
            return;
        }

        //visible when first building is unlocked whole and lab is minimum 5 level
        if (GameManager.instance.panels[9].GetComponent<SlotPanel>() != null)
        {
            for (int i = 0; i < 10; i++)
            {
                if (GameManager.instance.panels[i].GetComponent<BuySlotPanel>() != null)
                {
                    return;
                }
            }

            if(GameManager.instance.panels[9].GetComponent<SlotPanel>().slotLevel < 5)
            {
                return;
            }

            gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
            gameObject.GetComponent<CanvasGroup>().interactable = true;
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private void SetNumberOfBuildingText(int number)
    {
        if(GameManager.instance == null)
        {
            num_building.text = "Und";
            return;
        }

        num_building.text = number.ToString();
    }

    private void SetArrowActiveOrDisable(int numberOfBuilding)
    {
        //can move to next building only
        if(numberOfBuilding <= 1)
        {
            leftArrow.interactable = false;
            rightArrow.interactable = true;
        }
        else if (numberOfBuilding >= GameManager.instance.maxNumberOfBuilding)
        {
            leftArrow.interactable = true;
            rightArrow.interactable = false;
        }
        else
        {
            leftArrow.interactable = true;
            rightArrow.interactable = true;
        }
    }

    public void OnArrowLeftPressed()
    {
        if(GameManager.instance.numberOfBuilding > 1)
        {
            GameManager.instance.numberOfBuilding--;
            //set anim change building
            GameManager.instance.TurnSlotPanel(GameManager.instance.numberOfBuilding);
            SetNumberOfBuildingText(GameManager.instance.numberOfBuilding);
            SetArrowActiveOrDisable(GameManager.instance.numberOfBuilding);
        }

        //delete when second building is available
        if (GameManager.instance.numberOfBuilding > 1)
            GameManager.instance.BlockImageTurn(true);
        else
            GameManager.instance.BlockImageTurn(false);
    }

    public void OnArrowRightPressed()
    {
        if (GameManager.instance.numberOfBuilding < GameManager.instance.maxNumberOfBuilding)
        {
            GameManager.instance.numberOfBuilding++;
            //set anim change building
            GameManager.instance.TurnSlotPanel(GameManager.instance.numberOfBuilding);
            SetNumberOfBuildingText(GameManager.instance.numberOfBuilding);
            SetArrowActiveOrDisable(GameManager.instance.numberOfBuilding);
        }

        //delete when second building is available
        if (GameManager.instance.numberOfBuilding > 1)
            GameManager.instance.BlockImageTurn(true);
        else
            GameManager.instance.BlockImageTurn(false);
    }
}
