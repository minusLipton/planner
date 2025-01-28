using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace TimePlannerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
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
