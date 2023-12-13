using BackupSoftware.Model;
using BackupSoftware.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
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
            switchLanguage("en");
            DataContext = new BackupSoftware.ViewModel.BackupJob();

        }
        private void EncryptFilesWithSelectedExtension()
        {
            string sourceDirectoryPath = txt_sour.Text;
            if (FileExtensionComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedExtension = selectedItem.Content.ToString();

                if (Directory.Exists(sourceDirectoryPath) && !string.IsNullOrEmpty(selectedExtension))
                {
                    try
                    {
                        string[] files = Directory.GetFiles(sourceDirectoryPath, $"*{selectedExtension}");

                        if (files.Length > 0)
                        {
                            string outputDirectoryPath = @"E:\A3\System Progamation\Project(.Net)\XOR"; // Output directory path for encrypted files

                            foreach (string filePath in files)
                            {
                                EncryptAndSaveToFile(filePath, outputDirectoryPath);
                            }

                            MessageBox.Show($"Files with extension '{selectedExtension}' encrypted successfully and saved in the 'xor' folder.");
                        }
                        else
                        {
                            MessageBox.Show($"No files found with extension '{selectedExtension}' in the source directory.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Please provide a valid source directory and select a file extension.");
                }
            }
            else
            {
                MessageBox.Show("Please select a file extension.");
            }
        }

        private void EncryptAndSaveToFile(string filePath, string outputDirectoryPath)
        {
            char xorKey = 'P';

            try
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                string fileExtension = System.IO.Path.GetExtension(fileName);
                string uniqueIdentifier = DateTime.Now.ToString("yyyyMMddHHmmss");

                string outputFileName = $"{System.IO.Path.GetFileNameWithoutExtension(fileName)}_encrypted_{uniqueIdentifier}{fileExtension}";
                string outputFilePath = System.IO.Path.Combine(outputDirectoryPath, outputFileName);

                byte[] inputBytes = File.ReadAllBytes(filePath);
                byte[] encryptedBytes = new byte[inputBytes.Length];

                for (int i = 0; i < inputBytes.Length; i++)
                {
                    encryptedBytes[i] = (byte)(inputBytes[i] ^ xorKey);
                }

                File.WriteAllBytes(outputFilePath, encryptedBytes);

                Console.WriteLine($"File encrypted and saved at: {outputFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        private void switchLanguage(string LanguageCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();
            switch (LanguageCode)
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

        private void AddJob_Click(object sender, RoutedEventArgs e)
        {
            EncryptFilesWithSelectedExtension();

            string jobName = txt_job.Text;
            string sourcePath = txt_sour.Text;
            string destinationPath = txt_dest.Text;
            string backupType = txt_job_type.Text;


            Job newJob = new Job(jobName, sourcePath, destinationPath, backupType);

            if (Owner is MainWindow mainWindow)
            {
                mainWindow.Jobs.Add(newJob);
            }

            Close();
        }
    }
}
