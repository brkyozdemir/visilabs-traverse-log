using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace Visilabs.TraverseLog
{
    class Program
    {
        private static string corrupted;
        private static string modify;
        private static string value;
        static void Main(string[] args)
        {

            var appSettings = ConfigurationManager.AppSettings;
            corrupted = appSettings["Visilabs"];
            modify = appSettings["Modify"];

            DirectoryInfo startDir = new DirectoryInfo(corrupted);

            RecurseFileStructure recurseFileStructure = new RecurseFileStructure();
            Console.Write("Enter a file name: ");
            value = Console.ReadLine();
            recurseFileStructure.TraverseDirectory(startDir);

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }

        public class RecurseFileStructure
        {
            public void TraverseDirectory(DirectoryInfo directoryInfo)
            {
                var subdirectories = directoryInfo.EnumerateDirectories();

                foreach (var subdirectory in subdirectories)
                {
                    TraverseDirectory(subdirectory);
                }

                var files = directoryInfo.EnumerateFiles();
                int counter = 0;
                foreach (var file in files)
                {
                    if (file.Name.Contains(value))
                    {
                        counter += 1;
                        var percentage = (counter * 100) / files.Count();
                        HandleFile(file);
                        Console.WriteLine("%" + percentage + " of total files have been processed.");
                        Console.WriteLine("");
                    }
                }
            }

            void HandleFile(FileInfo file)
            {
                Console.WriteLine("{0}", file.Name);
                try
                {
                    string[] lines = File.ReadAllLines(corrupted + file.Name);
                    using (StreamWriter writer = new StreamWriter(modify + file.Name))
                    {
                        Console.WriteLine("Process has started...");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var percentage = (i * 100) / lines.Count();
                            Console.Write("\r%{0} ", percentage+1 + " of file is done.");
                            writer.WriteLine(lines[i]);
                        }
                        Console.WriteLine("Fully copied and pasted");
                        writer.Flush();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
            }
        }
    }
}
