using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TasksPopup : MonoBehaviour
{
    private Animator anim;
    private TasksManager tasksManager;
    private bool isShowed;
    
    public List<TaskPanel> currentTasks = new List<TaskPanel>();

    [SerializeField]
    private Text title;
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private TaskPanel taskPrefab;
    [SerializeField]
    private Button buildButton;
    [SerializeField]
    private Text buildButton_Text;

    private void Awake()
    {
        isShowed = false;
        anim = GetComponent<Animator>();
    }

    public void Show(TasksManager tasksManager)
    {
        this.tasksManager = tasksManager;
        anim.SetTrigger("Show");
        isShowed = true;

        title.text = LocalizationManager.instance.StringForKey("TasksPopup_Title");
        buildButton_Text.text = LocalizationManager.instance.StringForKey("TasksPopup_buildButtonText");

        GenerateTaskGO();

        buildButton.onClick.AddListener(() =>
        {
            this.tasksManager.GetComponent<BuySlotPanel>().OnBuyBusinessButtonPressed();
            Hide();
        });
    }

    private void GenerateTaskGO()
    {
        if(tasksManager == null)
        {
            Debug.LogError("TasksManager is equal null.");
            return;
        }
        for (int i = 0; i < tasksManager.tasks.Count; i++)
        {
            TaskPanel taskGO = Instantiate(taskPrefab, mainPanel.transform);
            taskGO.Initialize(tasksManager.tasks[i]);
            currentTasks.Add(taskGO);
        }
    }

    public void Hide()
    {
        anim.SetTrigger("Hide");
        isShowed = false;

        currentTasks.Clear();
        for (int i = 0; i < mainPanel.transform.childCount; i++)
        {
            Destroy(mainPanel.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if (isShowed)
        {
            buildButton.interactable = true;
            for (int i = 0; i < currentTasks.Count; i++)
            {
                if(currentTasks[i].status != true)
                {
                    buildButton.interactable = false;
                }
            }
        }
    }
}
