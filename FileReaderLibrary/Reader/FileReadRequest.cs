namespace FileReaderLibrary.Reader
{
    public class FileReadRequest
    {
        private readonly string filePath;

        public FileReadRequest(string filePath)
        {
            this.filePath = filePath;
            this.UseEncryption = false;
            this.UsePermissions = false;
            this.RoleName = string.Empty;
        }

        public string FilePath { get { return this.filePath; } }

        public bool UseEncryption { get; set; }

        public bool UsePermissions { get; set; }

        public string RoleName { get; set; }
    }
}
