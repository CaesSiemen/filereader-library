using System;
using System.IO.Abstractions;
using System.Text;

namespace FileReaderLibrary.Reader
{
    internal class FileReader : IFileReader
    {
        private readonly IFileSystem fileSystem;

        public FileReader() : this(new FileSystem()) { }

        public FileReader(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public string ReadFile(string filename)
        {
            if (!fileSystem.File.Exists(filename)) throw new ArgumentException("File not found.");

            var fileContent = new StringBuilder();

            using (var streamReader = fileSystem.File.OpenText(filename))
            {
                var line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    fileContent.Append(line + '\n');
                }
            }

            return fileContent.ToString().TrimEnd('\n');
        }
    }
}
