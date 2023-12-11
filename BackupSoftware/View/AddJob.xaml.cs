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
        private void fileencr()
        {
            string sourceFilePath = txt_sour.Text;
            if (FileExtensionComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedExtension = selectedItem.Content.ToString();

                if (!string.IsNullOrEmpty(sourceFilePath) && !string.IsNullOrEmpty(selectedExtension))
                {
                    // Get all files with the selected extension in the same directory
                    string directory = System.IO.Path.GetDirectoryName(sourceFilePath);
                    string[] files = Directory.GetFiles(directory, $"*{selectedExtension}");

                    if (files.Length > 0)
                    {
                        // Encrypt each file with the selected extension
                        foreach (string file in files)
                        {
                            EncryptFile(file);
                        }

                        MessageBox.Show($"Files with extension '.{selectedExtension}' encrypted successfully.");
                    }
                    else
                    {
                        MessageBox.Show($"No files found with extension '.{selectedExtension}' in the directory.");
                    }
                }
                else
                {
                    MessageBox.Show("Please provide a valid source file and select a file extension.");
                }
            }
        }

        private void EncryptFile(string filePath)
        {
            // Define XOR key
            // Any character value will work
            char xorKey = 'P';

            try
            {
                // Get the filename with extension from the original file path
                string fileName = System.IO.Path.GetFileName(filePath);

                // Get the file extension
                string fileExtension = System.IO.Path.GetExtension(fileName);

                // Generate a unique identifier (timestamp) to make the file name unique
                string uniqueIdentifier = DateTime.Now.ToString("yyyyMMddHHmmss");

                // Construct the output file name appending unique identifier before the extension
                string outputFileName = $"{fileName}encrypted{uniqueIdentifier}{fileExtension}";
                string outputFilePath = System.IO.Path.Combine("E:\\A3\\System Progamation\\Project(.Net)\\XOR", outputFileName);

                // Read all bytes from the input file
                byte[] inputBytes = File.ReadAllBytes(filePath);

                // Perform XOR operation of key with every byte in the file content
                byte[] encryptedBytes = new byte[inputBytes.Length];
                for (int i = 0; i < inputBytes.Length; i++)
                {
                    encryptedBytes[i] = (byte)(inputBytes[i] ^ xorKey);
                }

                // Write encrypted bytes to the output file (overwriting if it exists)
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
            fileencr();

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
