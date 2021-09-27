using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;

namespace JsonFileSorter.Models
{
    class Folder
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("folders")]
        public IList<Folder>? SubFolders { get; } = null;

        [JsonPropertyName("files")]
        public IList<string>? Files { get; } = null;

        [JsonIgnore]
        private Folder? Parent { get; set; } = null;

        [JsonConstructor]
        public Folder(string name, IList<Folder>? subFolders, IList<string>? files)
        {
            Name = name;
            SubFolders = subFolders;
            Files = files;

            if (SubFolders != null)
            {
                foreach (Folder folder in SubFolders)
                {
                    folder.Parent = this;
                }
            }
        }

        public Folder(DirectoryInfo dir, Folder? parent = null)
        {
            Name = dir.Name;

            IEnumerable<DirectoryInfo> subDirectories = dir.EnumerateDirectories();
            if (subDirectories.Any())
            {
                SubFolders = subDirectories.Select(d => new Folder(d, this)).ToList();
            }

            IEnumerable<FileInfo> files = dir.EnumerateFiles();
            if (files.Any())
            {
                Files = files.Select(f => f.Name).ToList();
            }

            Parent = parent;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            if (SubFolders != null)
            {
                foreach (Folder subFolder in SubFolders)
                {
                    foreach (KeyValuePair<string, string> file in subFolder)
                    {
                        yield return file;
                    }
                }
            }

            if (Files != null)
            {
                foreach (string fileName in Files)
                {
                    yield return new KeyValuePair<string, string>(fileName, Path);
                }
            }
        }

        [JsonIgnore]
        private string? path = null;
        [JsonIgnore]
        private string Path
        {
            get
            {
                if (path == null)
                {
                    path = $"{(Parent != null ? Parent.Path + "/" : "")}{Name}";
                }
                return path;
            }
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
