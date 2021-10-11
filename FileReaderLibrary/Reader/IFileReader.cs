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
        /// </summary>
        /// <param name="request">The Request object which holds the following information: Filepath, use of Encryption.</param>
        /// <returns>Returns the contents as a string.</returns>
        string ReadTextFile(FileReadRequest request);

        /// <summary>
        /// Read an XML file.
        /// This method can use the Permission functionality.
        /// </summary>
        /// <param name="request">The Request object which holds the following information: Filepath, use of permissions, Rolename if permissions are used.</param>
        /// <returns>Returns the contents as a XmlDocument.</returns>
        XmlDocument ReadXmlFile(FileReadRequest request);
    }
}
