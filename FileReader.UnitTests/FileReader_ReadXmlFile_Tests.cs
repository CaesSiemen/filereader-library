using FileReaderLibrary;
using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;
using System.Xml;

namespace FileReader.UnitTests
{
    public class FileReader_ReadXmlFile_Tests
    {
        private readonly string testFileContent = @"<test><firstline>line 1</firstline><secondline>line 2</secondline><thirdline>line 3</thirdline></test>";

        [SetUp]
        public void Setup()
        {
            var testfilePath = @"C:\xmlfile.xml";

            var mockFileSystem = new MockFileSystem();
            var mockFile = new MockFileData(testFileContent);
            mockFileSystem.AddFile(testfilePath, mockFile);

            FileReaderManager.Initiate(mockFileSystem);
        }

        [Test]
        public void Reading_XmlFile_Should_Return_XmlDocument()
        {
            // Arrange
            var testfilePath = @"C:\xmlfile.xml";
            var fileReader = FileReaderManager.RetrieveFileReader();

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(testFileContent);

            // Act
            var result = fileReader.ReadXmlFile(testfilePath);

            // Assert
            Assert.AreEqual(xmlDocument.DocumentElement.InnerText, result.DocumentElement.InnerText);
        }

        [Test]
        public void Read_Unexisting_File_Should_Throw()
        {
            // Arrange
            var testfilePath = @"C:\unexistingfile.xml";
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadTextFile(testfilePath));
        }

        [Test]
        public void Reading_Wrong_FileType_Should_Throw()
        {
            // Arrange
            var testfilePath = @"C:\xmlfile.txt";
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadXmlFile(testfilePath));
        }
    }
}
