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
        /// </summary>
        /// <param name="fileName">The file path of the textfile.</param>
        /// <param name="encrypted">Notify if the input file is encrypted or not.</param>
        /// <returns>Returns the contents as a string.</returns>
        string ReadTextFile(string fileName, bool encrypted = false);

        /// <summary>
        /// Read an XML file.
        /// </summary>
        /// <param name="fileName">The file path of the textfile.</param>
        /// <returns>Returns the contents as a XmlDocument.</returns>
        XmlDocument ReadXmlFile(string fileName);
    }
}
