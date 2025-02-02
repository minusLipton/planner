﻿using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TimePlannerApp
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadTasksFromJson(); // Load tasks from JSON when the application starts
        }

        // Add Task Button Click Event
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

            UpdateTotalTimeSpent(Tasks);
            UpdateProgressBar(Tasks);
        }

        // Delete Task Button Click Event
        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Example of deleting a task
            var taskToRemove = (Task)((Button)sender).DataContext;
            Tasks.Remove(taskToRemove);

            UpdateTotalTimeSpent(Tasks);
            UpdateProgressBar(Tasks);
        }

        // Filter Button Click Event
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected filter from the ComboBox
            string selectedFilter = ((ComboBoxItem)FilterComboBox.SelectedItem).Content.ToString();

            // Get the filtered tasks based on the selected filter
            ObservableCollection<Task> filteredTasks;

            switch (selectedFilter)
            {
                case "Dzienny": // Daily filter
                    filteredTasks = new ObservableCollection<Task>(Tasks.Where(t => t.StartTime.Date == DateTime.Now.Date));
                    break;

                case "Tygodniowy": // Weekly filter
                    var startOfWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(7);
                    filteredTasks = new ObservableCollection<Task>(Tasks.Where(t => t.StartTime >= startOfWeek && t.StartTime < endOfWeek));
                    break;

                case "Miesięczny": // Monthly filter
                    filteredTasks = new ObservableCollection<Task>(Tasks.Where(t => t.StartTime.Month == DateTime.Now.Month && t.StartTime.Year == DateTime.Now.Year));
                    break;

                default:
                    filteredTasks = new ObservableCollection<Task>(Tasks);
                    break;
            }

            // Update the TotalTimeSpentTextBlock for the filtered tasks
            UpdateTotalTimeSpent(filteredTasks);

            // Update the progress bar with the filtered tasks
            UpdateProgressBar(filteredTasks);
        }

        // Method to update total time spent
        private void UpdateTotalTimeSpent(ObservableCollection<Task> filteredTasks)
        {
            int totalMinutes = filteredTasks.Sum(t => t.HoursSpent * 60 + t.MinutesSpent);
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            TotalTimeSpentTextBlock.Text = $"{hours} hours {minutes} minutes";
        }

        // Method to update the progress bar (graph)
        private void UpdateProgressBar(ObservableCollection<Task> filteredTasks)
        {
            ProgressBarCanvas.Children.Clear();
            LegendStackPanel.Children.Clear();

            // Calculate total time for the filtered tasks
            int totalMinutes = filteredTasks.Sum(t => t.HoursSpent * 60 + t.MinutesSpent);

            // Initialize variables for drawing
            double currentX = 0;
            Random random = new Random();

            foreach (var task in filteredTasks)
            {
                int taskMinutes = task.HoursSpent * 60 + task.MinutesSpent;
                double taskWidth = (taskMinutes / (double)totalMinutes) * ProgressBarCanvas.Width;

                // Create a colored rectangle for the progress bar
                var rectangle = new Rectangle
                {
                    Width = taskWidth,
                    Height = 30,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)))
                };
                Canvas.SetLeft(rectangle, currentX);
                ProgressBarCanvas.Children.Add(rectangle);

                // Add task name to the legend
                var legendItem = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                var colorBox = new Border
                {
                    Background = rectangle.Fill,
                    Width = 20,
                    Height = 20,
                    Margin = new Thickness(0, 0, 5, 0)
                };
                legendItem.Children.Add(colorBox);
                legendItem.Children.Add(new TextBlock { Text = task.TaskName, Foreground = Brushes.White });
                LegendStackPanel.Children.Add(legendItem);

                currentX += taskWidth;
            }
        }

        // Method to save tasks to JSON file
        private void SaveTasksToJson()
        {
            string filePath = "tasks.json";
            var json = JsonConvert.SerializeObject(Tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Method to load tasks from JSON file
        private void LoadTasksFromJson()
        {
            string filePath = "tasks.json";
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var loadedTasks = JsonConvert.DeserializeObject<ObservableCollection<Task>>(json);
                Tasks.Clear();

                foreach (var task in loadedTasks)
                {
                    Tasks.Add(task);
                }
            }
        }

        // Event handler to save tasks when window is closed
        private void Window_Closed(object sender, EventArgs e)
        {
            SaveTasksToJson();
        }
    }

    // Task class that represents a task in the planner
    public class Task
    {
        public string TaskName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int HoursSpent { get; set; }
        public int MinutesSpent { get; set; }
    }
}
