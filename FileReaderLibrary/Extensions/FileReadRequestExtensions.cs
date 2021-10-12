using FileReaderLibrary.Reader;
using System.Collections.Generic;
using System.IO;

namespace FileReaderLibrary.Extensions
{
    internal static class FileReadRequestExtensions
    {
        private static readonly List<FileType> encryptionSupportedFileTypes = new List<FileType>() { FileType.Text, FileType.Xml, FileType.Json };
        private static readonly List<FileType> securitySupportedFileTypes = new List<FileType>() { FileType.Text, FileType.Xml, FileType.Json };


        internal static bool IsSecuritySupported(this FileReadRequest request)
        {
            return request.FileType.HasValue && securitySupportedFileTypes.Contains(request.FileType.Value) != false && request.UsePermissions != false;
        }

        internal static bool IsEncryptionSupported(this FileReadRequest request)
        {
            return request.FileType.HasValue && encryptionSupportedFileTypes.Contains(request.FileType.Value) != false && request.UseEncryption != false;
        }

        //internal static bool IsFileTypeCorrect(this FileReadRequest request)
        //{
        //    return request.FileType.HasValue && request.FileType.Value.GetEnumMemberValue() == Path.GetExtension(request.FilePath);
        //}
    }
}
