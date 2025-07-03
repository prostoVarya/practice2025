using CommandLib;
using System;
using System.IO;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var tempDir = Path.GetTempPath();
            
            var directorySizeCommand = new FileSystemCommands.DirectorySizeCommand(tempDir);
            directorySizeCommand.Execute();

            var findFilesCommand = new FileSystemCommands.FindFilesCommand(tempDir, "*.*");
            findFilesCommand.Execute();
        }
    }
}



