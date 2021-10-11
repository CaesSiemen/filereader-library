using FileReaderLibrary.Reader;
using System;
using System.IO.Abstractions;

namespace FileReaderLibrary
{
    /// <summary>
    /// This Manager manages the File Reader instance. It can also be used for instantiating a Reader with specific implementations.
    /// </summary>
    public static class FileReaderManager
    {
        private static IFileReader fileReader = null;

        /// <summary>
        /// Initiates the default File Reader.
        /// </summary>
        public static void Initiate()
        {
            fileReader = new FileReader();
        }

        /// <summary>
        /// Initiates the File Reader with a specific IFileSystem implementation.
        /// </summary>
        /// <param name="fileSystem">File system used by the File Reader.</param>
        public static void Initiate(IFileSystem fileSystem)
        {
            fileReader = new FileReader(fileSystem);
        }

        /// <summary>
        /// Retrieved the File Reader which has been initiated.
        /// </summary>
        /// <returns>The current File Reader.</returns>
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
