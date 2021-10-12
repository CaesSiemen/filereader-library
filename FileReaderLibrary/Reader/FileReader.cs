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
            if (!request.FileType.HasValue || request.FileType.Value != FileType.Text)
            {
                throw new ArgumentException("The provided file should be a text file.");
            }

            if (request.IsSecuritySupported() && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            return this.ExtractFileContent(request);
        }

        public XmlDocument ReadXmlFile(FileReadRequest request)
        {
            if (!request.FileType.HasValue || request.FileType.Value != FileType.Xml)
            {
                throw new ArgumentException("The provided file should be an xml file.");
            }

            if (request.IsSecuritySupported() && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            var fileContent = this.ExtractFileContent(request);

            return (XmlDocument)this.ParseFileContent(request.FileType.Value, fileContent);
        }

        public JsonDocument ReadJsonFile(FileReadRequest request)
        {
            if (!request.FileType.HasValue || request.FileType.Value != FileType.Json)
            {
                throw new ArgumentException("The provided file should be an json file.");
            }

            if (request.IsSecuritySupported() && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            var fileContent = this.ExtractFileContent(request);

            return (JsonDocument)this.ParseFileContent(request.FileType.Value, fileContent);
        }

        public object ReadFile(FileReadRequest request)
        {
            if (!request.FileType.HasValue)
            {
                throw new ArgumentException("The provided file type is not supported.");
            }

            if (request.IsSecuritySupported() && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            var fileContent = this.ExtractFileContent(request);

            return this.ParseFileContent(request.FileType.Value, fileContent);
        }


        private string ExtractFileContent(FileReadRequest request)
        {
            if (!fileSystem.File.Exists(request.FilePath)) throw new ArgumentException("File not found.");

            var strBuilder = new StringBuilder();
            using (var streamReader = fileSystem.File.OpenText(request.FilePath))
            {
                var line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    strBuilder.Append(line + '\n');
                }
            }

            var fileContent = strBuilder.ToString().TrimEnd('\n');

            if (request.IsEncryptionSupported())
                fileContent = this.encryptionHandler.DecryptFileContent(fileContent);

            return fileContent;
        }

        private object ParseFileContent(FileType fileType, string fileContent)
        {
            try
            {
                switch (fileType)
                {
                    case FileType.Text:
                        {
                            return fileContent;
                        }
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
    }
}
