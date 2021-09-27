using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace JsonFileSorter
{
    class Program
    {
        private const string HumanReadableFlag = "-h";
        private const string CopyFlag = "-c";

        static async Task Main(string[] args)
        {
            Regex flagRegex = new(@"-\w+");
            ISet<string> flags = args
                .Where(arg => flagRegex.IsMatch(arg))
                .Distinct()
                .ToHashSet();

            /*
             * generate <directory_path> [<output_file>|flags]
             */
            if (args.Length >= 2 && args[0].ToLower() == "generate")
            {
                string inputDirectory = args[1];
                if (Directory.Exists(inputDirectory))
                {
                    string? outputFile;
                    if (args.Length >= 3 && args[2].Length > 0 && !flagRegex.IsMatch(args[2]) )
                    {
                        outputFile = args[2];
                    }
                    else
                    {
                        DirectoryInfo inputDirInfo = new(inputDirectory);
                        outputFile = Path.Combine(inputDirInfo.FullName, inputDirInfo.Name + ".json");
                    }

                    SortGenerator sortGenerator = new(inputDirectory, outputFile);
                    await sortGenerator.GenerateFolder().SaveJsonAsync(flags.Contains(HumanReadableFlag));
                }
                else
                {
                    Console.WriteLine("Input directory doesn't exist.");
                }
            }

            /*
             * sort <sorting_json> <input_directory> <output_directory> [flags]
             */
            else if (args.Length >= 4 && args[0].ToLower() == "sort")
            {
                string jsonPath = args[1];
                string inputDirectory = args[2];
                string outputDirectory = args[3];

                bool jsonExists = File.Exists(jsonPath);
                bool inputDirectoryExists = Directory.Exists(inputDirectory);
                if (jsonExists && inputDirectoryExists)
                {
                    Sorter sorter = new(jsonPath, inputDirectory, outputDirectory);
                    sorter.Sort(flags.Contains(CopyFlag));
                }
                else
                {
                    if (!jsonExists)
                    {
                        Console.WriteLine("Json file doesn't exist.");
                    }
                    
                    if (!inputDirectoryExists)
                    {
                        Console.WriteLine("Input directory doesn't exist.");
                    }
                }
            }

            else
            {
                ShowUsage();
            }
        }

        public static void ShowUsage()
        {
            Console.WriteLine(@"
Usage:
    generate <directory> [<output_file>|flags]
        -h | Outputs JSON in a human readable format
    sort <sorting_json> <input_directory> <output_directory> [flags]
        -c | Copy files instead of moving them"
            );
        }
    }
}
