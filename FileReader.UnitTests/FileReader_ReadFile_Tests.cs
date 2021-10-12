using FileReaderLibrary;
using FileReaderLibrary.Reader;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;
using System.Xml;

namespace FileReader.UnitTests
{
    public class FileReader_ReadFile_Tests
    {
        private readonly string textfilePath = @"C:\testfile.txt";
        private readonly string jsonfilePath = @"C:\jsonfile.json";
        private readonly string xmlfilePath = @"C:\xmlfile.xml";
        private readonly string textfileContent = "test line 1\ntest line 2\test line 3\n test line 4";
        private readonly string jsonfileContent = "{\n\"test\":{\n\"line1\":\"this is line 1\",\n\"line2\":\"this is line 2\"\n}\n}";
        private readonly string xmlfileContent = @"<test><firstline>line 1</firstline><secondline>line 2</secondline><thirdline>line 3</thirdline></test>";

        [SetUp]
        public void Setup()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(textfilePath, new MockFileData(textfileContent));
            mockFileSystem.AddFile(jsonfilePath, new MockFileData(jsonfileContent));
            mockFileSystem.AddFile(xmlfilePath, new MockFileData(xmlfileContent));

            FileReaderManager.SetFileSystem(mockFileSystem);
        }

        [Test]
        public void Reading_Text_File_Should_Return_ContentString()
        {
            // Arrange
            var request = new FileReadRequest(textfilePath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadFile(request);

            // Assert
            Assert.IsAssignableFrom(typeof(string), result);
        }

        [Test]
        public void Reading_Json_File_Should_Return_JsonDocument()
        {
            // Arrange
            var request = new FileReadRequest(jsonfilePath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadFile(request);

            // Assert
            Assert.IsAssignableFrom(typeof(JsonDocument), result);
        }

        [Test]
        public void Reading_XML_File_Should_Return_XmlDocument()
        {
            // Arrange
            var request = new FileReadRequest(xmlfilePath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadFile(request);

            // Assert
            Assert.IsAssignableFrom(typeof(XmlDocument), result);
        }
    }
}
