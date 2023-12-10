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
            int numberOfJobs = 2;

            for (int i = 1; i <= numberOfJobs; i++)
            {
                //viewlan.chosingLanguage.translate();

                // Console.WriteLine(Properties.Resources.Job + $" {i}:");
                string jobName = txt_job.Text;

                // Console.WriteLine(Properties.Resources.Sou + $" {i}:");
                string sourcePath = txt_sour.Text;

                //   Console.WriteLine(Properties.Resources.Path + $"{i}:");
                string destinationPath = txt_dest.Text;

                //  Console.WriteLine(Properties.Resources.TypeBack + " " + Properties.Resources.TypeF + " / " + Properties.Resources.TypeD + " :");
                string backupType = txt_job_type.Text;

                while (string.IsNullOrEmpty(backupType) || (!backupType.Equals("Differential", StringComparison.OrdinalIgnoreCase) && !backupType.Equals("Full", StringComparison.OrdinalIgnoreCase)))
                {
                    //   Console.WriteLine(Properties.Resources.InputError);
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

                txt_job.Text = String.Empty;
                txt_sour.Text = String.Empty;
                /*txt_dest.Text = String.Empty;
                txt_job_type.Text = String.Empty;*/
            }

            for (int i = 1; i <= numberOfJobs; i++)
            {
                backupManager.GetLastBackupJob().RunBackupJob();
            }

            // Console.WriteLine(Properties.Resources.Exit);
            //Console.ReadKey();


        }
    }
}
