using BackupSoftware.Model;
using BackupSoftware.View;
using BackupSoftware.ViewModel;
using System.Collections.ObjectModel;
using System.IO;
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
            BackupManager backupManager = new BackupManager();

            InitializeComponent();
            switchLanguage("en");

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
        private void MenuClikItem(object sender, RoutedEventArgs e)
        {
            switchLanguage("en");
        }
        private void MenuClikItemfr(object sender, RoutedEventArgs e) {
            switchLanguage("fr");

        }

        private void switchLanguage(string LanguageCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            switch(LanguageCode)
            {
                case "en":
                    dictionary.Source = new Uri("..\\Dictionary.en.xaml", UriKind.Relative);
                    break;
                case "fr":
                    dictionary.Source = new Uri("..\\Dictionary.fr.xaml", UriKind.Relative);
                    break;
                default:
                    dictionary.Source = new Uri("..\\Dictionary.en.xaml", UriKind.Relative);
                    break;
            }
            this.Resources.MergedDictionaries.Add(dictionary);

        }

        private void RunJob_Click(object sender, RoutedEventArgs e)
        {

            // Handle the Run Job button click
            if (sender is Button button && button.Tag is BackupSoftware.Model.Job job)
            {
                // Perform the job execution logic here
                // For example: job.RunBackupJob();
                MessageBox.Show($"Running job: {job.Name}");

                string backupType = job.JobType;

                //  string message = jobName + " " + sourcePath + " " + destinationPath + " " + backupType;
                //MessageBox.Show(message);

                BackupSoftware.Model.Job job2 = new BackupSoftware.Model.Job(job.Name, job.Source, job.Destination, backupType);
                backupManager.AddBackupJob(job);
                ((BackupJob)DataContext).JobInstance = job;


                IBackupStrategy strategy = backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) ? new DifferentialBackup()
                    : (IBackupStrategy)new FullBackup();

                // Set the strategy for the last added job
                backupManager.GetLastBackupJob().SetBackupStrategy(strategy);




                backupManager.GetLastBackupJob().RunBackupJob();
            }
        }
    }
}
