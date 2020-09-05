using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WaitForTask
{
    public bool isComplete = false;
    string waitText;
    bool useDots;
    System.Threading.Tasks.Task task;

    public WaitForTask(System.Threading.Tasks.Task task, string WaitText = "", bool useDots = false)
    {
        this.task = task;
        this.waitText = WaitText;
        this.useDots = useDots;
    }
}