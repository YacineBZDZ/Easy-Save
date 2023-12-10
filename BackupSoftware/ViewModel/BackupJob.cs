using BackupSoftware.Model;
using System;
using System.IO;

namespace BackupSoftware.ViewModel
{
    class BackupJob
    {
        private Job jobInstance;
        private LogFile logFile;
        private IBackupStrategy backupStrategy;

        public BackupJob()
        {
            this.JobInstance = new Job("job1", "E:\\A3\\System Progamation\\Project(.Net)\\source", "E:\\A3\\System Progamation\\Project(.Net)\\destination", "full") ;
            this.logFile = new LogFile();

        }
        public Job JobInstance { get => jobInstance; set => jobInstance = value; }

        public void SetBackupStrategy(IBackupStrategy strategy)
        {
            this.backupStrategy = strategy;
        }

        public string RunBackupJob()
        {
            try
            {
                if (backupStrategy == null)
                {
                    return "Error: Backup strategy not set.";
                }

                string sourcePath = jobInstance.Source;
                string destinationPath = jobInstance.Destination;

                if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
                {
                    return "Error: Source or destination path is null or empty.";
                }

                sourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sourcePath);
                destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destinationPath);

                if (!Directory.Exists(sourcePath))
                {
                    return $"Error: Source directory does not exist - {sourcePath}";
                }

                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                string result = backupStrategy.Backup(sourcePath, destinationPath);
                logFile.WriteLogFile(jobInstance.Name, sourcePath, destinationPath, result);
                return result;
            }
            catch (Exception ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
        }
    }
}

