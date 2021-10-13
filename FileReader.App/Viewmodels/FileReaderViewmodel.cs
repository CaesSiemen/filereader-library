using FileReader.App.Viewmodels.Commands;
using FileReaderLibrary;
using FileReaderLibrary.Reader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace FileReader.App.Viewmodels
{
    internal class FileReaderViewmodel : ViewmodelBase
    {
        private string filePath;
        private string selectedFileType;
        private bool useEncryption;
        private bool useRoleBasedSecurity;
        private string roleName;
        private string fileContent;

        private ObservableCollection<string> availableFileTypes = new ObservableCollection<string>();
        private ReadOnlyCollection<string> roAvaliableTypes = null;

        private ICommand commandChooseFile;
        private ICommand commandReadFile;
        private ICommand commandClearInput;

        private IFileReader fileReader;

        public FileReaderViewmodel() : base("Custom File Reader")
        {
            fileReader = FileReaderManager.RetrieveFileReader();
            var fileTypeNames = Enum.GetNames(typeof(FileType));
            foreach (var name in fileTypeNames)
            {
                availableFileTypes.Add(name);
            }
        }

        public ReadOnlyCollection<string> AvailableFileTypes
        {
            get { return roAvaliableTypes ?? (roAvaliableTypes = new ReadOnlyCollection<string>(availableFileTypes)); }
        }

        public string SelectedFileType
        {
            get { return selectedFileType; }
            set
            {
                if (selectedFileType == value) return;
                selectedFileType = value;
                OnPropertyChanged();
            }
        }

        public string FilePath
        {
            get { return filePath; }
            private set
            {
                if (filePath == value) return;
                filePath = value;
                OnPropertyChanged();
            }
        }

        public bool UseEncryption
        {
            get { return useEncryption; }
            set
            {
                if (useEncryption == value) return;
                useEncryption = value;
                OnPropertyChanged();
            }
        }

        public bool UseRoleBasedSecurity
        {
            get { return useRoleBasedSecurity; }
            set
            {
                if (useRoleBasedSecurity == value) return;
                useRoleBasedSecurity = value;
                if (value == false) this.RoleName = string.Empty;
                OnPropertyChanged();
            }
        }

        public string RoleName
        {
            get { return roleName; }
            set
            {
                if (roleName == value) return;
                roleName = value;
                OnPropertyChanged();
            }
        }

        public string FileContent
        {
            get { return fileContent; }
            private set
            {
                if (fileContent == value) return;
                fileContent = value;
                OnPropertyChanged();
            }
        }

        public ICommand CommandChooseFile
        {
            get { return commandChooseFile ?? (commandChooseFile = new RelayCommand(x => ChooseFile(), x => true)); }
        }

        private void ChooseFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;

            var result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
                this.FilePath = openFileDialog.FileName;
        }

        public ICommand CommandClearInput
        {
            get { return commandClearInput ?? (commandClearInput = new RelayCommand(x => ClearInput(), x => CanClearInput())); }
        }

        private bool CanClearInput()
        {
            if (!string.IsNullOrEmpty(filePath)
                || !string.IsNullOrEmpty(selectedFileType)
                || useEncryption
                || useRoleBasedSecurity
                || !string.IsNullOrEmpty(roleName))
            {
                return true;
            }
            return false;
        }

        private void ClearInput()
        {
            FilePath = string.Empty;
            UseEncryption = false;
            UseRoleBasedSecurity = false;
            RoleName = string.Empty;
        }

        public ICommand CommandReadFile
        {
            get { return commandReadFile ?? (commandReadFile = new RelayCommand(x => ReadFile(), x => CanReadFile())); }
        }

        public bool CanReadFile()
        {
            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(selectedFileType)
                && (useRoleBasedSecurity == !string.IsNullOrEmpty(roleName)))
            {
                return true;
            }
            return false;
        }

        public void ReadFile()
        {
            try
            {
                var request = new FileReadRequest(filePath)
                {
                    UseEncryption = useEncryption,
                    UsePermissions = useRoleBasedSecurity,
                    RoleName = useRoleBasedSecurity ? roleName : string.Empty
                };

                var chosenFileType = Enum.Parse<FileType>(selectedFileType);
                switch (chosenFileType)
                {
                    case FileType.Text:
                        {
                            FileContent = fileReader.ReadTextFile(request);
                            break;
                        }
                    case FileType.Xml:
                        {
                            var content = fileReader.ReadXmlFile(request);
                            using (StringWriter stringWriter = new StringWriter())
                            using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                            {
                                content.WriteTo(xmlWriter);
                                FileContent = stringWriter.ToString();
                            }
                            break;
                        }
                    case FileType.Json:
                        {
                    var content = fileReader.ReadJsonFile(request);
                    FileContent = content.RootElement.ToString();
                    break;
                }
                default:
                        break;
            };
        }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); 
            }
}
    }
}
