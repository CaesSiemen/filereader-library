namespace FileReaderLibrary.Encryption
{
    public interface IEncryptionHandler
    {
        public string DecryptFileContent(string fileContent);
    }
}
