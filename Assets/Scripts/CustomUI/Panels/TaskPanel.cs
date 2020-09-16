using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TaskPanel : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text info;
    [SerializeField]
    private Image statusImage;

    public Sprite[] statusSprites;
    public IconAndName[] icons;
    private TaskToBildFloor task;

    public bool status = false;
    private bool iconIsSet;

    public void Initialize(TaskToBildFloor task)
    {
        this.task = task;
    }

    void Update()
    {
        if(task.Type == TaskType.CASH)
            CashTask();

        if (task.Type == TaskType.FLOOR_ENTITY_COLLECTION)
            FloorEntityCollectionTask(task.IndexOfFloor);

        if (task.Type == TaskType.FLOOR_LEVEL)
            FloorLevelTask(task.IndexOfFloor);

        if (task.Type == TaskType.HAS_MANAGER)
            HasManagerTask();
    }

    private void CashTask()
    {
        //icon...
        if (icon.sprite == null)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                if (icons[i].name.Equals(TaskType.CASH.ToString()))
                {
                    icon.sprite = icons[i].icon;
                    break;
                }
            }
        }

        info.text = string.Format("{0}/ {1}", NumberFormatter.ToString(PlayerManager.instance.cash, false, true), NumberFormatter.ToString(task.ValueToCollect, false, true));
        if (PlayerManager.instance.cash < task.ValueToCollect)
        {
            statusImage.sprite = statusSprites[0];
            info.color = GameColors.disableColorForButtons;
            status = false;
        }
        else
        {
            statusImage.sprite = statusSprites[1];
            info.color = GameColors.availableColorGreen;
            status = true;
        }
    }

    private void FloorEntityCollectionTask(int indexOfFloor)
    {
        if (!iconIsSet)
        {
            icon.sprite = GameManager.instance.slotPanelSymbols[indexOfFloor % 10];
            iconIsSet = true;
        }

        task.CurrentValueOfCollection = PlayerManager.instance.GetValueSlotsCounter(indexOfFloor);
        info.text = string.Format("Entity: {0}({1})", NumberFormatter.ToString(task.ValueToCollect, false, false, false), NumberFormatter.ToString(task.CurrentValueOfCollection, false, false, false));
        if (task.CurrentValueOfCollection < task.ValueToCollect)
        {
            statusImage.sprite = statusSprites[0];
            info.color = GameColors.disableColorForButtons;
            status = false;
        }
        else
        {
            statusImage.sprite = statusSprites[1];
            info.color = GameColors.availableColorGreen;
            status = true;
        }
    }

    private void FloorLevelTask(int indexOfFloor)
    {
        if (!iconIsSet)
        {
            icon.sprite = GameManager.instance.slotPanelSymbols[indexOfFloor % 10];
            iconIsSet = true;
        }

        task.CurrentValueOfCollection = PlayerManager.instance.GetSlot(indexOfFloor).level;
        info.text = string.Format("Level: {0}({1})", task.ValueToCollect, task.CurrentValueOfCollection);
        if (task.CurrentValueOfCollection < task.ValueToCollect)
        {
            statusImage.sprite = statusSprites[0];
            info.color = GameColors.disableColorForButtons;
            status = false;
        }
        else
        {
            statusImage.sprite = statusSprites[1];
            info.color = GameColors.availableColorGreen;
            status = true;
        }
    }

    private void HasManagerTask()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public struct IconAndName
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite icon;
}
