using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

enum TaskStatus
{
    Pending,
    InProgress,
    Completed
}
class TaskItem
{
    public string Title { get; set; }
    public TaskStatus Status { get; set; }
}

class Program
{
    static List<TaskItem> tasks = new List<TaskItem>();
    static string filePath = "tasks.json";

    static void Main()
    {
        LoadTasks();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Task Manager\n");
            Console.WriteLine("1 - Add Task");
            Console.WriteLine("2 - View Tasks");
            Console.WriteLine("3 - Start Task");
            Console.WriteLine("4 - Complete Task");
            Console.WriteLine("5 - Exit");
            Console.Write("\nChoose option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;

                case "2":
                    ViewTasks();
                    break;

                case "3":
                    StartTask();
                    break;

                case "4":
                    CompleteTask();
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    static void AddTask()
    {
        Console.Write("\nEnter task title: ");
        string title = Console.ReadLine();

        tasks.Add(new TaskItem
        {
            Title = title,
            Status = TaskStatus.Pending
        });
        SaveTasks();

        Console.WriteLine("Task added.");
        Pause();
    }

    static void ViewTasks()
    {
        Console.WriteLine("\nTasks:\n");

        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks yet.");
        }
        else
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i].Title} [{tasks[i].Status}]");
            }
        }

        Pause();
    }
    static void StartTask()
    {
        ViewTasks();

        if (tasks.Count == 0)
            return;

        Console.Write("\nEnter task number to start: ");

        if (int.TryParse(Console.ReadLine(), out int number))
        {
            if (number >= 1 && number <= tasks.Count)
            {
                tasks[number - 1].Status = TaskStatus.InProgress;
                SaveTasks();
                Console.WriteLine("Task marked as In Progress.");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        Pause();
    }
    static void CompleteTask()
    {
        ViewTasks();

        if (tasks.Count == 0)
            return;

        Console.Write("\nEnter task number to complete: ");

        if (int.TryParse(Console.ReadLine(), out int number))
        {
            if (number >= 1 && number <= tasks.Count)
            {
                tasks[number - 1].Status = TaskStatus.Completed;
                RunAutomation(tasks[number - 1]);
                SaveTasks();
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task number.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        Pause();
    }
    static void LoadTasks()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            tasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
    }
    static void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, json);
    }
    static void RunAutomation(TaskItem task)
    {
        if (task.Status == TaskStatus.Completed)
        {
            string msg = $"Automation triggered: '{task.Title}' completed at {DateTime.Now}";
            Console.WriteLine(msg);
            LogAutomation(msg);
        }
    }
    static void LogAutomation(string message)
    {
        File.AppendAllText("automation_log.txt", message + Environment.NewLine);
    }

    static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}