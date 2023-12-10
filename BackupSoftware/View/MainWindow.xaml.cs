using BackupSoftware.Model;
using BackupSoftware.View;
using BackupSoftware.ViewModel;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BackupSoftware
{
    public partial class MainWindow : Window
    {
        BackupManager backupManager = new BackupManager();

        public ObservableCollection<BackupSoftware.Model.Job> Jobs { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new BackupSoftware.ViewModel.BackupJob();
            Jobs = new ObservableCollection<BackupSoftware.Model.Job>();
            JobList.ItemsSource = Jobs;
        }

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            // Open AddJob window and add the created job to Jobs collection
            AddJob addJobWindow = new AddJob();
            addJobWindow.Owner = this;
            addJobWindow.ShowDialog();

            // When AddJob window closes, add the job to the list
            if (addJobWindow.DataContext is BackupSoftware.Model.Job newJob)
            {
                Jobs.Add(newJob);
                backupManager.AddBackupJob(newJob);

                ((BackupJob)DataContext).JobInstance = newJob;

            }
        }

        private void RunJob_Click(object sender, RoutedEventArgs e)
        {
            // Handle the Run Job button click
            if (sender is Button btn && btn.Tag is Job job)
            {
                // Perform the job execution logic here
                // For example: job.RunBackupJob();

                // Assuming RunBackupJob method exists in BackupManager to run the job
                backupManager.GetLastBackupJob().RunBackupJob();

            }
        }
    }
}
