using FileSystemCommands;
using System;
using System.IO;
using Xunit;

public class FileSystemCommandsTests : IDisposable
{
    private readonly string _testDir;

    public FileSystemCommandsTests()
    {
        _testDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDir))
        {
            Directory.Delete(_testDir, true);
        }
    }

    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        File.WriteAllText(Path.Combine(_testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(_testDir, "test2.txt"), "World");

        var command = new DirectorySizeCommand(_testDir);
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        File.WriteAllText(Path.Combine(_testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(_testDir, "file2.log"), "Log");

        var command = new FindFilesCommand(_testDir, "*.txt");
        var exception = Record.Exception(() => command.Execute());
        Assert.Null(exception);
    }
}
