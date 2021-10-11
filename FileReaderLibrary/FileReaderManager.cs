using FileReaderLibrary.Encryption;
using FileReaderLibrary.Permissions;
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
        private static FileReader fileReader = null;
       
        /// <summary>
        /// Override the filesystem used by the File Reader.
        /// </summary>
        /// <param name="fileSystem"></param>
        public static void SetFileSystem(IFileSystem fileSystem)
        {
            EnsureFileReaderExists();
            fileReader.SetFileSystem(fileSystem);
        }
        
        /// <summary>
        /// Override the encryptionhandler used by the File Reader.
        /// </summary>
        /// <param name="encryptionHandler"></param>
        public static void SetEncryptionHandler(IEncryptionHandler encryptionHandler)
        {
            EnsureFileReaderExists();
            fileReader.SetEncryptionHandler(encryptionHandler);
        }

        /// <summary>
        /// Override the permissionshandler used by the File Reader.
        /// </summary>
        /// <param name="permissionshandler"></param>
        public static void SetPermissionsHandler(IPermissionsHandler permissionshandler)
        {
            EnsureFileReaderExists();
            fileReader.SetPermissionsHandler(permissionshandler);
        }

        /// <summary>
        /// Retrieved the File Reader which has been initiated.
        /// </summary>
        /// <returns>The current File Reader.</returns>
        public static IFileReader RetrieveFileReader()
        {
            EnsureFileReaderExists();
            return fileReader;
        }

        private static void EnsureFileReaderExists()
        {
            if (fileReader is null) fileReader = new FileReader();
        }
    }
}
