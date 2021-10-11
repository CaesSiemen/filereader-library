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

        public FileReader() : this(new FileSystem()) { }

        public FileReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public string ReadTextFile(string fileName)
        {
            if (!IsFileTypeCorrect(FileType.Text, fileName))
            {
                throw new ArgumentException("The provided file should be a text file.");
            }

            return this.ReadFile(fileName);
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
            //using var stringWriter = new StringWriter();
            //using var xmlTextWriter = new XmlTextWriter(stringWriter);

            //doc.WriteTo(xmlTextWriter);

            return doc;
        }

        private string ReadFile(string fileName)
        {
            if (!fileSystem.File.Exists(fileName)) throw new ArgumentException("File not found.");

            var fileContent = new StringBuilder();

            using (var streamReader = fileSystem.File.OpenText(fileName))
            {
                var line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    fileContent.Append(line + '\n');
                }
            }

            return fileContent.ToString().TrimEnd('\n');
        }

        private bool IsFileTypeCorrect(FileType fileType, string fileName)
        {
            if (fileType.GetEnumMeberValue() == Path.GetExtension(fileName)) return true;
            return false;
        }
    }
}
