using FileReaderLibrary.Encryption;
using FileReaderLibrary.Extensions;
using FileReaderLibrary.Permissions;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
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
            if (!IsFileTypeCorrect(FileType.Text, request.FilePath))
            {
                throw new ArgumentException("The provided file should be a text file.");
            }

            if (request.UsePermissions && !permissionsHandler.HasReadPermission(request.RoleName))
            {
                throw new UnauthorizedAccessException("Unauthorized to read this file.");
            }

            return this.ReadFile(request.FilePath, request.UseEncryption);
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

            var fileContent = this.ReadFile(request.FilePath, request.UseEncryption);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(fileContent);

            return doc;
        }

        private string ReadFile(string fileName, bool isEncrypted = false)
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

            if (isEncrypted)
                fileContent = this.encryptionHandler.DecryptFileContent(fileContent);

            return fileContent;
        }

        private bool IsFileTypeCorrect(FileType fileType, string fileName)
        {
            if (fileType.GetEnumMemberValue() == Path.GetExtension(fileName)) return true;
            return false;
        }
    }
}
