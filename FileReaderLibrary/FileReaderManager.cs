using FileReaderLibrary.Reader;
using System;
using System.IO.Abstractions;

namespace FileReaderLibrary
{
    public static class FileReaderManager
    {
        private static bool isInitaited;
        private static IFileReader fileReader = null;

        public static void Initiate()
        {
            if (!isInitaited)
            {
                fileReader = new FileReader();
                isInitaited = true;
            }
        }

        public static void Initiate(IFileSystem fileSystem)
        {
            if (!isInitaited)
            {
                fileReader = new FileReader(fileSystem);
                isInitaited = true;
            }
        }


        public static IFileReader RetrieveFileReader()
        {
            if (fileReader is null)
            {
                throw new InvalidOperationException("File Reader has not been initiated.");
            }

            return fileReader;
        }
    }
}
