using CommandLib;
using System;
using System.IO;

namespace FileSystemCommands
{
    public class DirectorySizeCommand : ICommand
    {
        private readonly string _directoryPath;

        public DirectorySizeCommand(string directoryPath)
        {
            _directoryPath = directoryPath;
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {_directoryPath}");
            }

            long totalSize = 0;
            var files = Directory.GetFiles(_directoryPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                totalSize += new FileInfo(file).Length;
            }

            Console.WriteLine($"Total size of directory '{_directoryPath}': {totalSize} bytes");
        }
    }

    public class FindFilesCommand : ICommand
    {
        private readonly string _directoryPath;
        private readonly string _searchPattern;

        public FindFilesCommand(string directoryPath, string searchPattern)
        {
            _directoryPath = directoryPath;
            _searchPattern = searchPattern;
        }

        public void Execute()
        {
            if (!Directory.Exists(_directoryPath))
            {
                throw new DirectoryNotFoundException($"Directory not found: {_directoryPath}");
            }

            var files = Directory.GetFiles(_directoryPath, _searchPattern, SearchOption.TopDirectoryOnly);
            Console.WriteLine($"Files matching '{_searchPattern}' in directory '{_directoryPath}':");
            foreach (var file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}
