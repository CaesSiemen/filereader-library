using FileReaderLibrary;
using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;

namespace FileReader.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            var testfilePath = @"C:\testfile.txt";
            var testfileContent = "test line 1\ntest line 2\test line 3\n test line 4";

            var mockFileSystem = new MockFileSystem();
            var mockFile = new MockFileData(testfileContent);
            mockFileSystem.AddFile(testfilePath, mockFile);

            FileReaderManager.Initiate(mockFileSystem);
        }

        [Test]
        public void Reading_File_Should_Return_ContentString()
        {
            // Arrange
            var testfilePath = @"C:\testfile.txt";
            var testfileContent = "test line 1\ntest line 2\test line 3\n test line 4";
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadFile(testfilePath);

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
            Assert.Throws<ArgumentException>(() => fileReader.ReadFile(filePath)); 
        }
    }
}