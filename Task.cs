using System;
using System.Collections.Generic;

public class Task
{
    public string Name { get; set; }
    public Action TaskAction { get; set; }
    public bool IsCompleted { get; set; }

    public Task(string name, Action action)
    {
        Name = name;
        TaskAction = action;
        IsCompleted = false;
    }

    public void Execute()
    {
        TaskAction();
    }
}
