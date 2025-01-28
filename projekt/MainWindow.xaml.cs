using System.Windows;

namespace TimePlannerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj dodaj logikę dodawania zadania
            MessageBox.Show("Dodaj zadanie kliknięte!");
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj dodaj logikę usuwania zadania
            MessageBox.Show("Usuń zadanie kliknięte!");
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj dodaj logikę filtrowania zadań
            MessageBox.Show("Filtruj kliknięte!");
        }
    }
}
