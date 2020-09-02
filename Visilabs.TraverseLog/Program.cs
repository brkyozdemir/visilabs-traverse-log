using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace Visilabs.TraverseLog
{
    class Program
    {
        private static string corrupted1;
        private static string corrupted2;
        private static string corrupted3;
        private static string corrupted4;
        private static string modify;
        private static string value;
        static void Main(string[] args)
        {

            var appSettings = ConfigurationManager.AppSettings;
            corrupted1 = appSettings["Visilabs1"];
            corrupted2 = appSettings["Visilabs2"];
            corrupted3 = appSettings["Visilabs3"];
            corrupted4 = appSettings["Visilabs4"];
            modify = appSettings["Modify"];

            Console.Write("Enter a file name: ");
            value = Console.ReadLine();

            DirectoryInfo startDir = new DirectoryInfo(corrupted1);
            RecurseFileStructure recurseFileStructure = new RecurseFileStructure();
            recurseFileStructure.TraverseDirectory(startDir);

            DirectoryInfo startDir2 = new DirectoryInfo(corrupted2);
            RecurseFileStructure recurseFileStructure2 = new RecurseFileStructure();
            recurseFileStructure2.TraverseDirectory(startDir2);

            DirectoryInfo startDir3 = new DirectoryInfo(corrupted3);
            RecurseFileStructure recurseFileStructure3 = new RecurseFileStructure();
            recurseFileStructure3.TraverseDirectory(startDir3);

            DirectoryInfo startDir4 = new DirectoryInfo(corrupted4);
            RecurseFileStructure recurseFileStructure4 = new RecurseFileStructure();
            recurseFileStructure4.TraverseDirectory(startDir4);

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
                    Console.WriteLine("subdirectory" + subdirectory);
                    TraverseDirectory(subdirectory);
                }

                var files = directoryInfo.EnumerateFiles();
                int counter = 0;
                foreach (var file in files)
                {
                    Console.WriteLine("file" + file);
                    if (file.Name.Contains(value))
                    {
                        counter += 1;
                        var percentage = (counter * 100) / files.Count();
                        HandleFile(file, directoryInfo);
                        Console.WriteLine("%" + percentage + " of total files have been processed.");
                        Console.WriteLine("");
                    }
                }

            }

            void HandleFile(FileInfo file, DirectoryInfo directory)
            {
                Console.WriteLine("{0}", file.Name);
                Console.WriteLine(directory.Name);
                try
                {
                    string[] lines = File.ReadAllLines(directory + "\\" + file.Name);
                    using (StreamWriter writer = new StreamWriter(modify + file.Name))
                    {
                        Console.WriteLine("Process has started...");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            var percentage = (i * 100) / lines.Count();
                            Console.Write("\r%{0} ", percentage + 1 + " of file is done.");
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
