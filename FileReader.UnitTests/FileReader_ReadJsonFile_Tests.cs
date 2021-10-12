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
        private readonly string testfileEcryptedPath = @"C:\encryptedjsonfile.json";
        private readonly string testfileInvalidPath = @"C:\invalidjsonfile.json";
        private readonly string testfileContent = "{\n\"test\":{\n\"line1\":\"this is line 1\",\n\"line2\":\"this is line 2\"\n}\n}";
        private readonly string testfileReverseContent = "}\n}\n\"2 enil si siht\":\"2enil\"\n,\"1 enil si siht\":\"1enil\"\n{:\"tset\"\n{";
        private readonly string testfileInvalidContent = "invalid";

        [SetUp]
        public void Setup()
        {
            var mockFileSystem = new MockFileSystem();
            mockFileSystem.AddFile(testfilePath, new MockFileData(testfileContent));
            mockFileSystem.AddFile(testfileEcryptedPath, new MockFileData(testfileReverseContent));
            mockFileSystem.AddFile(testfileInvalidPath, new MockFileData(testfileInvalidContent));

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
        public void Reading_Unexisting_File_Should_Throw()
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

        [Test]
        public void Reading_Encrypted_File_Should_Return_DecryptedFileContent()
        {
            // Arrange
            var request = new FileReadRequest(testfileEcryptedPath);
            request.UseEncryption = true;
            var fileReader = FileReaderManager.RetrieveFileReader();

            var jsonDocument = JsonDocument.Parse(testfileContent);
            // Act
            var result = fileReader.ReadJsonFile(request);

            // Assert
            Assert.AreEqual(result.RootElement.ToString(), jsonDocument.RootElement.ToString());
        }

        [Test]
        public void Reading_Invalid_File_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(testfileInvalidPath);
            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<InvalidOperationException>(() => fileReader.ReadJsonFile(request));
        }

        [Test]
        public void Using_Correct_Role_Should_Return_JsonDocument()
        {
            // Arrange
            var request = new FileReadRequest(testfilePath);
            request.UsePermissions = true;
            request.RoleName = "ADMIN";

            var fileReader = FileReaderManager.RetrieveFileReader();

            var jsonDocument = JsonDocument.Parse(testfileContent);

            // Act
            var result = fileReader.ReadJsonFile(request);

            // Assert
            Assert.AreEqual(result.RootElement.ToString(), jsonDocument.RootElement.ToString());
        }

        [Test]
        public void Using_InCorrect_Role_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(testfilePath);
            request.UsePermissions = true;
            request.RoleName = "TEMPORARY";

            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<UnauthorizedAccessException>(() => fileReader.ReadJsonFile(request));
        }

        [Test]
        public void Using_No_Role_Should_Throw()
        {
            // Arrange
            var request = new FileReadRequest(testfilePath);
            request.UsePermissions = true;
            request.RoleName = string.Empty;

            var fileReader = FileReaderManager.RetrieveFileReader();

            // Act + Assert
            Assert.Throws<UnauthorizedAccessException>(() => fileReader.ReadJsonFile(request));
        }

    }
}
