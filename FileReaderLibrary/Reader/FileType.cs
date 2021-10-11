using System.Runtime.Serialization;

namespace FileReaderLibrary.Reader
{
    internal enum FileType
    {
        [EnumMember(Value = ".txt")]
        Text,

        [EnumMember(Value = ".xml")]
        Xml
    }
}
