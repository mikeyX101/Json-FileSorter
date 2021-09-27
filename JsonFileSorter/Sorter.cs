using JsonFileSorter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonFileSorter
{
    class Sorter
    {
        private Folder FolderSorting { get; }

        private DirectoryInfo InputDirectory { get; }

        private DirectoryInfo OutputDirectory { get; }

        public Sorter(string jsonPath, string inputDirectory, string outputDirectory)
        {
            string jsonString = File.ReadAllText(jsonPath);
            FolderSorting = JsonSerializer.Deserialize<Folder>(jsonString) 
                ?? throw new ArgumentNullException(nameof(jsonPath), "Specified JSON is invalid.");

            InputDirectory = new(inputDirectory);

            OutputDirectory = new(outputDirectory);
            OutputDirectory.Create();
        }

        public void Sort(bool copy)
        {
            Action<string, string> operation; // Input file, output directory
            if (copy)
            {
                operation = (filePath, outputDirectoryPath) =>
                {
                    File.Copy(filePath, outputDirectoryPath, true);
                };
            }
            else // Move
            {
                operation = (filePath, outputDirectoryPath) =>
                {
                    File.Move(filePath, outputDirectoryPath, true);
                };
            }

            // File name, Destination path
            foreach (KeyValuePair<string, string> file in FolderSorting)
            {
                string inputFilePath = $@"{InputDirectory.FullName}/{file.Key}";
                if (File.Exists(inputFilePath))
                {
                    string directoryPath = $@"{OutputDirectory.FullName}/{file.Value}";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    operation(inputFilePath, $@"{directoryPath}/{file.Key}");
                }
            }
        }
    }
}
