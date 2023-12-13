﻿using BackupSoftware.Model;
using System;
using System.Collections.Generic;

namespace BackupSoftware.ViewModel
{
    class BackupManager
    {
        private List<BackupJob> backupJobs;

        public BackupManager()
        {
            backupJobs = new List<BackupJob>();
        }
        public List<BackupJob> BackupJobs { get => backupJobs; set => backupJobs = value; }

        public BackupJob GetLastBackupJob()
        {
            return backupJobs.LastOrDefault();
        }

        public void AddBackupJob(Job jobInstance)
        {
           
                var backupJob = new BackupJob();
                backupJob.JobInstance = jobInstance;
                backupJobs.Add(backupJob);
            
            
        }
    }
}



