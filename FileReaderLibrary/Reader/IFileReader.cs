using System.Text.Json;
using System.Xml;

namespace FileReaderLibrary.Reader
{
    /// <summary>
    /// This File Reader is used to read a number a diffrent file types.
    /// </summary>
    public interface IFileReader
    {
        /// <summary>
        /// Read a text file.
        /// This method can use the Encryption functionality.
        /// This method can use the Permission functionality.
        /// </summary>
        /// <param name="request">The Request object which holds the following information: Filepath, use of Encryption, use of permissions, and a rolename if permissions are used.</param>
        /// <returns>Returns the contents as a string.</returns>
        string ReadTextFile(FileReadRequest request);

        /// <summary>
        /// Read an XML file.
        /// This method can use the Encryption functionality.
        /// This method can use the Permission functionality.
        /// </summary>
        /// <param name="request">The Request object which holds the following information: Filepath, use of Encryption, use of permissions, and a rolename if permissions are used.</param>
        /// <returns>Returns the contents as a XmlDocument.</returns>
        XmlDocument ReadXmlFile(FileReadRequest request);

        /// <summary>
        /// Read a JSON file.
        /// </summary>
        /// <param name="request">The Request object which holds the following information: Filepath.</param>
        /// <returns>Returns the contents as a JsonDocument.</returns>
        JsonDocument ReadJsonFile(FileReadRequest request);

        /// <summary>
        /// Read a file.
        /// Current supported filetypes are: Text, Xml and Json.
        /// </summary>
        /// <param name="request">Filepath, use of Encryption, use of permissions, and a rolename if permissions are used.</param>
        /// <returns>Returns the contents as a JsonDocument.</returns>
        object ReadFile(FileReadRequest request);
    }
}
