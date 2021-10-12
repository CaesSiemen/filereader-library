using FileReaderLibrary.Encryption;
using FileReaderLibrary.Extensions;
using FileReaderLibrary.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Text.Json;
using System.Xml;

namespace FileReaderLibrary.Reader
{
    internal class FileReader : IFileReader
    {
        private readonly List<FileType> encryptionSupportedFileTypes = new List<FileType>() { FileType.Text, FileType.Xml };
        private readonly List<FileType> securitySupportedFileTypes = new List<FileType>() { FileType.Text, FileType.Xml };

        private IFileSystem fileSystem;
        private IEncryptionHandler encryptionHandler;
        private IPermissionsHandler permissionsHandler;

        internal FileReader() : this(new FileSystem(), new DefaultEncryptionHandler(), new DefaultPermissionHandler()) { }

        internal FileReader(IFileSystem fileSystem, IEncryptionHandler encryptionHandler, IPermissionsHandler permissionsHandler)
        {
            this.fileSystem = fileSystem;
            this.encryptionHandler = encryptionHandler;
            this.permissionsHandler = permissionsHandler;
        }

        internal void SetFileSystem(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        internal void SetEncryptionHandler(IEncryptionHandler encryptionHandler)
        {
            this.encryptionHandler = encryptionHandler;
        }

        internal void SetPermissionsHandler(IPermissionsHandler permissionsHandler)
        {
            this.permissionsHandler = permissionsHandler;
        }


        public string ReadTextFile(FileReadRequest request)
        {
            if (!IsFileTypeCorrect(FileType.Text, request.FilePath))
            {
                throw new ArgumentException("The provided file should be a text file.");
            }

            if (request.UsePermissions && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            return this.ExtractFileContent(request.FilePath, FileType.Text, request.UseEncryption);
        }

        public XmlDocument ReadXmlFile(FileReadRequest request)
        {
            if (!IsFileTypeCorrect(FileType.Xml, request.FilePath))
            {
                throw new ArgumentException("The provided file should be an xml file.");
            }

            if(request.UsePermissions && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            var fileContent = this.ExtractFileContent(request.FilePath, FileType.Xml, request.UseEncryption);

            return (XmlDocument)this.ParseFileContent(FileType.Xml, fileContent);
        }

        public JsonDocument ReadJsonFile(FileReadRequest request)
        {
            if (!IsFileTypeCorrect(FileType.Json, request.FilePath))
            {
                throw new ArgumentException("The provided file should be an xml file.");
            }

            var fileContent = this.ExtractFileContent(request.FilePath, FileType.Json);

            return (JsonDocument)this.ParseFileContent(FileType.Json, fileContent);
        }

        public object ReadFile(FileType fileType, FileReadRequest request)
        {
            if (!IsFileTypeCorrect(fileType, request.FilePath))
            {
                throw new ArgumentException("The provided file should be an xml file.");
            }

            if (IsSecuritySupported(fileType, request) && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            var fileContent = this.ExtractFileContent(request.FilePath, fileType, request.UseEncryption);

            return this.ParseFileContent(fileType, fileContent);
        }


        private string ExtractFileContent(string fileName, FileType fileType, bool isEncrypted = false)
        {
            if (!fileSystem.File.Exists(fileName)) throw new ArgumentException("File not found.");

            var strBuilder = new StringBuilder();
            using (var streamReader = fileSystem.File.OpenText(fileName))
            {
                var line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    strBuilder.Append(line + '\n');
                }
            }

            var fileContent = strBuilder.ToString().TrimEnd('\n');

            if (IsEncryptionSupported(fileType, isEncrypted) is true)
                fileContent = this.encryptionHandler.DecryptFileContent(fileContent);

            return fileContent;
        }

        private object ParseFileContent(FileType fileType, string fileContent)
        {
            try
            {
                switch (fileType)
                {
                    case FileType.Json:
                        {
                            return JsonDocument.Parse(fileContent);
                        }
                    case FileType.Xml:
                        {
                            var xmlDocument = new XmlDocument();
                            xmlDocument.LoadXml(fileContent);
                            return xmlDocument;
                        }
                    default:
                        throw new InvalidOperationException("No filetype has been specified.");
                }
            }
            catch (Exception ex)
            {
                if (ex is XmlException || ex is JsonException)
                    throw new InvalidOperationException($"Something went wrong trying to parse the {fileType.ToString()}-file.");
                else
                    throw;
            }
        }


        private bool IsFileTypeCorrect(FileType fileType, string fileName)
        {
            if (fileType.GetEnumMemberValue() == Path.GetExtension(fileName)) return true;
            return false;
        }

        private bool IsSecuritySupported(FileType fileType, FileReadRequest request)
        {
            if( securitySupportedFileTypes.Contains(fileType) == false || request.UsePermissions == false) return false;
            return true;
        }

        private bool IsEncryptionSupported(FileType fileType, bool isEncrypted)
        {
            if (encryptionSupportedFileTypes.Contains(fileType) == false || isEncrypted == false) return false;

            return true;
        }
    }
}
