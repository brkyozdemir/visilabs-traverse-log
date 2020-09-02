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
                    var files = subdirectory.EnumerateFiles();
                    int counter = 0;
                    foreach (var file in files)
                    {
                        if (file.Name.Contains(value))
                        {
                            counter += 1;
                            var percentage = (counter * 100) / files.Count();
                            HandleFile(file, subdirectory);
                            Console.WriteLine("%" + percentage + " of total files have been processed.");
                            Console.WriteLine("");
                        }
                    }
                    TraverseDirectory(subdirectory);
                }


            }

            void HandleFile(FileInfo file, DirectoryInfo directory)
            {
                Console.WriteLine("{0}", file.Name);
                Console.WriteLine(directory.Name);
                try
                {
                    string[] lines = File.ReadAllLines(directory + "\\" + file.Name);
                    Directory.CreateDirectory(modify + directory.Name);
                    using (StreamWriter writer = new StreamWriter(modify + directory.Name + "\\" + file.Name))
                    {
                        Console.WriteLine("Process has started...");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var percentage = (i * 100) / lines.Count();
                            Console.Write("\r%{0} ", percentage + " of file is done.");
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
