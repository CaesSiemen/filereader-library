using System;

namespace FileReaderLibrary.Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLocation = @"C:\Users\Siemen Caes\source\repos\FileReader\Testing\test.txt";
            FileReaderManager.Initiate();

            var fileReader = FileReaderManager.RetrieveFileReader();

            Console.WriteLine(fileReader.ReadTextFile(fileLocation));


            Console.WriteLine(fileReader.ReadXmlFile(@"C:\Users\Siemen Caes\source\repos\FileReader\Testing\xmltest.xml").InnerXml);
        }()
    }
}
