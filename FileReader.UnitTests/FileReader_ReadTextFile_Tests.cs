using FileReaderLibrary;
using FileReaderLibrary.Reader;
using NUnit.Framework;
using System;
using System.IO.Abstractions.TestingHelpers;

namespace FileReader.UnitTests
{
    public class FileReader_ReadTextFile_Tests
    {
        private readonly string testfilePath = @"C:\testfile.txt";
        private readonly string testfileEcryptedPath = @"C:\encryptedtextfile.txt";
        private readonly string testfileContent = "test line 1\ntest line 2\test line 3\n test line 4";
        private readonly string testfileReverseContent = "4 enil tset \n3 enil tse\t2 enil tset\n1 enil tset";

        [SetUp]
        public void Setup()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(testfilePath, new MockFileData(testfileContent));
            mockFileSystem.AddFile(testfileEcryptedPath, new MockFileData(testfileReverseContent));

            FileReaderManager.SetFileSystem(mockFileSystem);
        }

        [Test]
        public void Reading_File_Should_Return_ContentString()
        {
            // Arrange
            var request = new FileReadRequest(testfilePath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadTextFile(request);

            // Assert
            Assert.AreEqual(result, testfileContent);
        }

        [Test]
        public void Read_Unexisting_File_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(@"C:\unexistingfile.txt");
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadTextFile(request));
        }

        [Test]
        public void Reading_Wrong_FileType_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(@"C:\textfile.anything");
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<ArgumentException>(() => fileReader.ReadTextFile(request));
        }

        [Test]
        public void Reading_Encrypted_File_Should_Return_DecryptedFileContent()
        {
            // Arrange
            var request = new FileReadRequest(testfileEcryptedPath);
            request.UseEncryption = true;
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act
            var result = fileReader.ReadTextFile(request);

            // Assert
            Assert.AreEqual(result, testfileContent);
        }
    }
}