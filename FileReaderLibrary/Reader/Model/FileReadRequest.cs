using FileReaderLibrary.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace FileReaderLibrary.Reader
{
    public class FileReadRequest
    {
        private readonly string filePath;
        private readonly FileType? fileType;

        public FileReadRequest(string filePath)
        {
            var extension = Path.GetExtension(filePath);
            var fieldInfo = typeof(FileType).GetFields().FirstOrDefault(x => x.GetAttributes<EnumMemberAttribute>(false).Any(x => x.Value == extension));

            if (fieldInfo is null)
            {
                fileType = null;
            }
            else
            {
                this.fileType = Enum.Parse<FileType>(fieldInfo.Name);
            }

            this.filePath = filePath;
            this.UseEncryption = false;
            this.UsePermissions = false;
            this.RoleName = string.Empty;
        }

        public string FilePath { get { return this.filePath; } }

        public FileType? FileType { get { return this.fileType; } }

        public bool UseEncryption { get; set; }

        public bool UsePermissions { get; set; }

        public string RoleName { get; set; }

    }
}
