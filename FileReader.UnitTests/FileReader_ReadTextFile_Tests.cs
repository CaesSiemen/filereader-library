using FileReaderLibrary;
using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;

namespace FileReader.UnitTests
{
    public class FileReader_ReadTextFile_Tests
    {
        private readonly string testfilePath = @"C:\testfile.txt";
        private readonly string testfileContent = "test line 1\ntest line 2\test line 3\n test line 4";

        [SetUp]
        public void Setup()
        {
            var mockFileSystem = new MockFileSystem();
            var mockFile = new MockFileData(testfileContent);
            mockFileSystem.AddFile(testfilePath, mockFile);

            FileReaderManager.Initiate(mockFileSystem);
        }

        [Test]
        public void Reading_File_Should_Return_ContentString()
        {
            // Arrange
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadTextFile(testfilePath);

            // Assert
            Assert.AreEqual(result, testfileContent);
        }

        [Test]
        public void Read_Unexisting_File_Should_Throw()
        {
            // Arrange
            var filePath = @"C:\unexistingfile.txt";

            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadTextFile(filePath));
        }

        [Test]
        public void Reading_Wrong_FileType_Should_Throw()
        {
            // Arrange
            var testfilePath = @"C:\textfile.anything";
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadXmlFile(testfilePath));
        }
    }
}