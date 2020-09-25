using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationScript : MonoBehaviour
{
    public Text cashText;
    private Animator cashTextAnim;

    public List<InfoBuilding> infoBuildings = new List<InfoBuilding>();

    private double cash = 2000000;
    
    private void Awake()
    {
        if(cashText != null)
        {
            cashTextAnim = cashText.GetComponent<Animator>();
        }    
    }

    private void Start()
    {
        foreach (var infoBuilding in infoBuildings)
        {
            infoBuilding.NameText.text = infoBuilding.name;
            infoBuilding.NumberOfFloorText.text = "Floors: " + infoBuilding.floorUncovered + " / 10";
            infoBuilding.cashPerSecondText.text = "$: " + infoBuilding.cashPerSecond + " /s";
        }
    }

    private void IncrementFakeCash(float value)
    {
        cash += value;
        cashTextAnim.SetTrigger("GetMoney");
    }

    private void Update()
    {
        cashText.text = NumberFormatter.ToString(cash, true, true, true);

        IncrementFakeCash(1156);
    }
}

[Serializable]
public struct InfoBuilding
{
    public string name;
    public Text NameText;
    public int floorUncovered;
    public Text NumberOfFloorText;
    public string cashPerSecond;
    public Text cashPerSecondText;
}
