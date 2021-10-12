using FileReaderLibrary;
using FileReaderLibrary.Reader;
using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;
using System.Text.Json;

namespace FileReader.UnitTests
{
    public class FileReader_ReadJsonFile_Tests
    {
        private readonly string testfilePath = @"C:\jsonfile.json";
        private readonly string testfileContent = "{\n\"test\":{\n\"line1\":\"this is line 1\",\n\"line2\":\"this is line 2\"\n}\n}";

        [SetUp]
        public void Setup()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(testfilePath, new MockFileData(testfileContent));

            FileReaderManager.SetFileSystem(mockFileSystem);
        }

        [Test]
        public void Reading_File_Should_Return_JsonDocument()
        {
            // Arrange
            var request = new FileReadRequest(testfilePath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            var jsonDocument = JsonDocument.Parse(testfileContent);
            // Act
            var result = fileReader.ReadJsonFile(request);

            // Assert
            Assert.AreEqual(result.RootElement.ToString(), jsonDocument.RootElement.ToString());
        }

        [Test]
        public void Read_Unexisting_File_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(@"C:\unexistingfile.json");
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadJsonFile(request));
        }

        [Test]
        public void Reading_Wrong_FileType_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(@"C:\jsonfile.anything");
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadJsonFile(request));
        }
    }
}
