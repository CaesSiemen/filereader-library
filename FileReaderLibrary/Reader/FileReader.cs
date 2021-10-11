using FileReaderLibrary.Encryption;
using FileReaderLibrary.Extensions;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Xml;

namespace FileReaderLibrary.Reader
{
    internal class FileReader : IFileReader
    {
        private readonly IFileSystem fileSystem;
        private readonly IEncryptionHandler encryptionHandler;

        public FileReader() : this(new FileSystem(), new DefaultEncryptionHandler()) { }

        public FileReader(IFileSystem fileSystem) : this(fileSystem, new DefaultEncryptionHandler()) { }

        public FileReader(IEncryptionHandler encryptionHandler) : this(new FileSystem(), encryptionHandler) { }

        public FileReader(IFileSystem fileSystem, IEncryptionHandler encryptionHandler)
        {
            this.fileSystem = fileSystem;
            this.encryptionHandler = encryptionHandler;
        }

        public string ReadTextFile(string fileName, bool isEncrypted = false)
        {
            if (!IsFileTypeCorrect(FileType.Text, fileName))
            {
                throw new ArgumentException("The provided file should be a text file.");
            }

            return this.ReadFile(fileName, isEncrypted);
        }

        public XmlDocument ReadXmlFile(string fileName)
        {
            if (!IsFileTypeCorrect(FileType.Xml, fileName))
            {
                throw new ArgumentException("The provided file should be an xml file.");
            }

            var fileContent = this.ReadFile(fileName);

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
