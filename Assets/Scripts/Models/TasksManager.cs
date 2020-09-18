using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType { CASH, FLOOR_ENTITY_COLLECTION, FLOOR_LEVEL, HAS_MANAGER }
public class TasksManager : MonoBehaviour
{
    public List<TaskToBildFloor> tasks = new List<TaskToBildFloor>();
    private BuySlotPanel buySlotPanel;

    private void Awake()
    {
        buySlotPanel = GetComponent<BuySlotPanel>();    
    }

    private void Start()
    {
        if(buySlotPanel != null)
            GenerateTasks(buySlotPanel.index);
    }

    public void GenerateTasks(int index)
    {
        switch (index)
        {
            //shop
            case 1:
                GenerateCashTask();
                GenerateCollectEntityTask(20);
                GenerateReachLevelTask(20);
                break;
            //cinema
            case 2:
                GenerateCashTask();
                GenerateCollectEntityTask(10);
                GenerateCollectEntityTask(100,2);
                break;
            //factory
            case 3:
                GenerateCashTask();
                GenerateCollectEntityTask(3);
                GenerateReachLevelTask(20);
                break;
            //library
            case 4:
                GenerateCollectEntityTask(3);
                GenerateReachLevelTask(15);
                break;
            //office
            case 5:
                GenerateReachLevelTask(10);
                GenerateReachLevelTask(25,2);
                break;
            //icecream
            case 6:
                GenerateReachLevelTask(5);
                GenerateReachLevelTask(15, 2);
                break;
            //gym
            case 7:
                GenerateReachLevelTask(5);
                GenerateCollectEntityTask(3,2);
                break;
            //radio
            case 8:
                GenerateReachLevelTask(5);
                GenerateReachLevelTask(15,2);
                GenerateReachLevelTask(25,3);
                break;
            //lab
            case 9:
                GenerateReachLevelTask(5);
                GenerateReachLevelTask(10, 2);
                GenerateReachLevelTask(15, 3);
                break;
            default:
                GenerateCashTask();
                GenerateCollectEntityTask(5);
                GenerateReachLevelTask(5);
                break;
        }
    }

    private void GenerateCashTask()
    {
        tasks.Add(new TaskToBildFloor
        {
            Name = "Collect Cash",
            Type = TaskType.CASH,
            ValueToCollect = buySlotPanel.cost,
            CurrentValueOfCollection = PlayerManager.instance.cash,
        });
    }

    private void GenerateCollectEntityTask(int value, int numberOfPreviousFloors = 1)
    {
        tasks.Add(new TaskToBildFloor
        {
            Name = "Collect entity of previous floor",
            Type = TaskType.FLOOR_ENTITY_COLLECTION,
            ValueToCollect = value,
            CurrentValueOfCollection = PlayerManager.instance.GetValueSlotsCounter(buySlotPanel.index - numberOfPreviousFloors),
            IndexOfFloor = buySlotPanel.index - numberOfPreviousFloors
        });
    }

    private void GenerateReachLevelTask(int value, int numberOfPreviousFloors = 1)
    {
        tasks.Add(new TaskToBildFloor
        {
            Name = "Get level of previous floor",
            Type = TaskType.FLOOR_LEVEL,
            ValueToCollect = value,
            CurrentValueOfCollection = PlayerManager.instance.GetSlot(buySlotPanel.index - numberOfPreviousFloors).level,
            IndexOfFloor = buySlotPanel.index - numberOfPreviousFloors
        });
    }
}
