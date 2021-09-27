using JsonFileSorter.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonFileSorter
{
    class SortGenerator
    {
        private DirectoryInfo Input { get; }

        private FileInfo OutputFile { get; }

        private Folder? Output { get; set; }

        public SortGenerator(string inputDirectory, string outputFile)
        {
            Input = new(inputDirectory);
            OutputFile = new(outputFile);
        }

        public SortGenerator GenerateFolder()
        {
            Output = new(Input);

            return this;
        }

        public async Task SaveJsonAsync(bool humanReadable)
        {
            if (Output != null)
            {
                JsonSerializerOptions options = new()
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = humanReadable
                };

                using FileStream outStream = OutputFile.Create();
                await JsonSerializer.SerializeAsync(outStream, Output, options);
            }
            else
            {
                throw new InvalidOperationException("Folder object was null. Call GenerateFolder() to generate it.");
            }
        }
    }
}
