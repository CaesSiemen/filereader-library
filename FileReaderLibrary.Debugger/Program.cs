using System;

namespace FileReaderLibrary.Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLocation = @"C:\Users\Siemen Caes\source\repos\FileReader\Testing\test.txt";
            var fileReader = FileReaderManager.RetrieveFileReader();

            Console.WriteLine(fileReader.ReadFile(fileLocation));
        }
    }
}
