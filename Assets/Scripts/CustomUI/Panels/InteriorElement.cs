using System;
using UnityEngine;

public enum InteriorElementStatus { NOT_AVAILABLE, AVAILABLE, BOUGHT }
public class InteriorElement : MonoBehaviour
{
    public InteriorElementStatus status = InteriorElementStatus.NOT_AVAILABLE;
    public float price;
    public Sprite symbol;

    public InteriorElementUI interiorElementUI;

    public void Initialize(Slot slot, int i)
    {
        interiorElementUI.Initialize(slot, i);
    }

    public void TurnOnElement()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //var color = spriteRenderer.color;
        //color.a = 1f;
        //spriteRenderer.color = color;
        Color desactiveColor = new Color(255, 255, 255, 255);
        spriteRenderer.color = desactiveColor;

        if (spriteRenderer.transform.childCount > 0)
        {
            for (int i = 0; i < spriteRenderer.transform.childCount; i++)
            {
                spriteRenderer.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void TurnOffElement()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        //var color = spriteRenderer.color;
        //color.a = 0f;
        //spriteRenderer.color = color;
        Color desactiveColor = new Color(0, 0, 0, 255);
        spriteRenderer.color = desactiveColor;

        if (spriteRenderer.transform.childCount > 0)
        {
            for (int i = 0; i < spriteRenderer.transform.childCount; i++)
            {
                if(spriteRenderer.transform.GetChild(i).GetComponent<InteriorElementUI>() == null)
                    spriteRenderer.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
