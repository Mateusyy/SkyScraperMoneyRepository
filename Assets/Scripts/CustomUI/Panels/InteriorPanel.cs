using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorPanel : MonoBehaviour
{
    public List<InteriorElement> interiorObjects = new List<InteriorElement>();

    public void Initialize(Slot slot)
    {
        for (int i = 0; i < interiorObjects.Count; i++)
        {
            interiorObjects[i].Initialize(slot, i);
            GeneratePrice(interiorObjects[i], slot, i);

            if (slot.GetInformationsAboutObjectToUnlock(i))
            {
                interiorObjects[i].status = InteriorElementStatus.BOUGHT;
            }

            if(interiorObjects[i].status != InteriorElementStatus.BOUGHT)
            {
                if (slot.level < slot.GetMilestoneLevelTarget(i))
                {
                    interiorObjects[i].status = InteriorElementStatus.NOT_AVAILABLE;
                    interiorObjects[i].interiorElementUI.TurnOnUiElements(false);
                }
                else
                {
                    interiorObjects[i].status = InteriorElementStatus.AVAILABLE;
                    interiorObjects[i].interiorElementUI.TurnOnUiElements(true);
                }

                interiorObjects[i].TurnOffElement();
            }
            else
            {
                interiorObjects[i].TurnOnElement();
                interiorObjects[i].interiorElementUI.TurnOffUiElements();
            }
        }
    }

    private void GeneratePrice(InteriorElement interiorElement, Slot slot, int i)
    {
        interiorElement.price = slot.ProfitForLevel(slot.GetMilestoneLevelTarget(i) * 2);
    }
}
