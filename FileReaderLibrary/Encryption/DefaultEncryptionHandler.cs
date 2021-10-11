namespace FileReaderLibrary.Encryption
{
    internal class DefaultEncryptionHandler : IEncryptionHandler
    {
        public string DecryptFileContent(string fileContent)
        {
            var charArray = fileContent.ToCharArray();
            var reverseString = string.Empty;

            for (int i = charArray.Length - 1; i > -1; i--)
            {
                reverseString += charArray[i];
            }

            return reverseString;
        }
    }
}
