# Json-FileSorter
C# CLI Application that generates JSON files listing directories and file names to later sort files with the same directory structure.

This was mainly made to sort textures from Pokemon XD/XG dumped with xxHash filenames by Dolphin, which is why a premade JSON file is already included for that case, but this could be used for anything really.

**Untested on MacOS and Linux, but builds are still provided.**

## Usage
```
generate <directory> [<output_file>|flags]
  -h | Outputs JSON in a human readable format
sort <sorting_json> <input_directory> <output_directory> [flags]
  -c | Copy files instead of moving them
```

## Download
See the release section.

Self-contained builds are provided for Windows, MacOS and Linux, all for x64 systems. If you download one of these builds, you don't need the .NET 5 runtime.

The framework dependent build requires the .NET 5 runtime to be installed, but will work on any x64 system.
MacOS and Linux users can run this build with the .NET CLI:
```
dotnet JsonFileSorter.dll <args>
```

## Build
You can either use Visual Studio 2019 or the .NET CLI to build using the solution.
