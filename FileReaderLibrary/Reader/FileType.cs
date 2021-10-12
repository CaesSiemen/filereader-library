using System.Runtime.Serialization;

namespace FileReaderLibrary.Reader
{
    public enum FileType
    {
        [EnumMember(Value = ".txt")]
        Text,

        [EnumMember(Value = ".xml")]
        Xml,

        [EnumMember(Value = ".json")]
        Json
    }
}
