using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TimePlannerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        private const string TasksFilePath = "tasks.json"; // Path for the tasks JSON file

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTasks();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Example of adding a new task
            Tasks.Add(new Task
            {
                TaskName = "Example Task",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(2).AddMinutes(30),
                HoursSpent = 2,
                MinutesSpent = 30
            });

            UpdateTotalTimeSpent();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Example of deleting a task
            var taskToRemove = (Task)((Button)sender).DataContext;
            Tasks.Remove(taskToRemove);
            UpdateTotalTimeSpent();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle filtering tasks based on daily, weekly, or monthly
            MessageBox.Show("Filtruj clicked!");
        }

        private void UpdateTotalTimeSpent()
        {
            int totalMinutes = Tasks.Sum(t => t.HoursSpent * 60 + t.MinutesSpent);
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;

            TotalTimeSpentTextBlock.Text = $"{hours} hours {minutes} minutes";
        }

        private void LoadTasks()
        {
            if (File.Exists(TasksFilePath))
            {
                var json = File.ReadAllText(TasksFilePath);
                var tasks = JsonConvert.DeserializeObject<ObservableCollection<Task>>(json);
                if (tasks != null)
                {
                    foreach (var task in tasks)
                    {
                        Tasks.Add(task);
                    }
                }
            }
        }

        private void SaveTasks()
        {
            var json = JsonConvert.SerializeObject(Tasks, Formatting.Indented);
            File.WriteAllText(TasksFilePath, json);
        }

        // Event handler for closing the application (Save Tasks)
        protected override void OnClosed(EventArgs e)
        {
            SaveTasks();
            base.OnClosed(e);
        }
    }

    public class Task
    {
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
    }
}