using CommandLib;
using System;
using System.Reflection;

namespace CommandRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var directorySizeCommand = new FileSystemCommands.DirectorySizeCommand("C:\Users\Dell\Desktop\practice2025");
            directorySizeCommand.Execute();

            var findFilesCommand = new FileSystemCommands.FindFilesCommand("C:\Users\Dell\Desktop\practice2025", "*.txt");
            findFilesCommand.Execute();
        }
    }
}

