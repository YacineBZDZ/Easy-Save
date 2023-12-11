using BackupSoftware.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading; 

namespace BackupSoftware.ViewModel
{
    class BackupJob
    {
        private Job jobInstance;
        private LogFile logFile;
        private IBackupStrategy backupStrategy;
        private string softwarePackageToDetect;
        public BackupJob()
        {
            this.jobInstance = new Job("", "", "", "");
            this.logFile = new LogFile();
            this.softwarePackageToDetect = "";
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
                if (IsSoftwarePackageRunning())
                {
                    return $"Error: The software package '{softwarePackageToDetect}' is running. Backup job aborted.";
                }

                
                lock (this)
                {
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
            }
            catch (Exception ex)
            {
                return $"Error in backup job: {ex.Message}";
            }
        }

        private bool IsSoftwarePackageRunning()
        {
            var processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(softwarePackageToDetect));
            return processes.Length > 0;
        }
    }
}