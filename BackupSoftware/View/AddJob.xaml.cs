using BackupSoftware.Model;
using BackupSoftware.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BackupSoftware.View
{
    /// <summary>
    /// Interaction logic for AddJob.xaml
    /// </summary>
    public partial class AddJob : Window
    {
        BackupManager backupManager = new BackupManager();

        public AddJob()
        {
            InitializeComponent();
            DataContext = new BackupSoftware.ViewModel.BackupJob();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            
                //viewlan.chosingLanguage.translate();

                string jobName = txt_job.Text;

                string sourcePath = txt_sour.Text;

                string destinationPath = txt_dest.Text;

                string backupType = txt_job_type.Text;

                while (string.IsNullOrEmpty(backupType) || (!backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) && !backupType.Equals("Full", StringComparison.OrdinalIgnoreCase)))
                {
                    backupType = txt_job_type.Text;
                }
                string message = jobName + " " + sourcePath + " " + destinationPath + " " + backupType;
                MessageBox.Show(message);

                BackupSoftware.Model.Job job = new BackupSoftware.Model.Job(jobName, sourcePath, destinationPath, backupType);
                backupManager.AddBackupJob(job);
                ((BackupJob)DataContext).JobInstance = job;


                IBackupStrategy strategy = backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) ? new DifferentialBackup()
                    : (IBackupStrategy)new FullBackup();

                // Set the strategy for the last added job
                backupManager.GetLastBackupJob().SetBackupStrategy(strategy);


            
           
                backupManager.GetLastBackupJob().RunBackupJob();

            txt_job.Text = String.Empty;
            txt_sour.Text = String.Empty;
            txt_dest.Text = String.Empty;
            txt_job_type.Text = String.Empty;

           


        }
        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            // Get job details from text boxes or input controls
            string jobName = txt_job.Text;
            string sourcePath = txt_sour.Text;
            string destinationPath = txt_dest.Text;
            string backupType = txt_job_type.Text;

            // Validate job details if needed

            // Create a new job instance
            Job newJob = new Job(jobName, sourcePath, destinationPath, backupType);

            // Pass the new job instance back to the MainWindow
            if (Owner is MainWindow mainWindow)
            {
                mainWindow.Jobs.Add(newJob);
            }

            // Close the AddJob window
            Close();
        }
    }
}
