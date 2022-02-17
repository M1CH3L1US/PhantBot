using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Infrastructure.Storage;
using Xunit;

namespace Infrastructure.Tests.Storage;

public class ThreadSafeFileWriterTest {
  private readonly IFileSystem _fileSystem;
  private readonly ThreadSafeFileWriter _sut;
  public string FileContent = "HiHi :3";
  public string FilePath = "C:/Temp/TestFile.txt";

  public ThreadSafeFileWriterTest(IFileSystem fileSystem) {
    _fileSystem = fileSystem;
    var timeout = TimeSpan.FromHours(100);
    _sut = new ThreadSafeFileWriter(fileSystem, timeout);
  }

  private IFile File => _fileSystem.File;

  [Fact]
  public async Task FileExists_ReturnsTrue_WhenFileExists() {
    await _sut.CreateFile(FilePath);
    _sut.FileExists(FilePath).Should().BeTrue();
  }

  [Fact]
  public void FileExists_ReturnsFalse_WhenFileDoesNotExist() {
    _sut.FileExists("FooBar").Should().BeFalse();
  }

  [Fact]
  public async Task CreateFile_ShouldCreateFile_WhenFileDoesNotExist() {
    await _sut.CreateFile(FilePath);

    File.Exists(FilePath).Should().BeTrue();
  }

  [Fact]
  public async Task CreateFile_ShouldNotOverwriteFile_WhenFileExists() {
    await _sut.CreateFile(FilePath);
    await File.WriteAllTextAsync(FilePath, FileContent);

    await _sut.CreateFile(FilePath);

    var contentAfterAdditionalCreate = await File.ReadAllTextAsync(FilePath);
    contentAfterAdditionalCreate.Should().Be(FileContent);
  }

  [Fact]
  public async Task WriteToFile_ShouldWriteToFile_WhenFileExists() {
    await _sut.CreateFile(FilePath);

    await _sut.WriteToFile(FilePath, FileContent);

    var contentAfterWrite = await File.ReadAllTextAsync(FilePath);
    contentAfterWrite.Should().Be(FileContent);
  }

  [Fact]
  public async Task WriteToFile_ShouldCreateFile_WhenFileDoesNotExist() {
    await _sut.WriteToFile(FilePath, FileContent);

    File.Exists(FilePath).Should().BeTrue();
  }

  [Fact]
  public async Task ReadFromFile_ShouldReadFileContent_WhenFileContentExists() {
    await _sut.CreateFile(FilePath);
    await File.WriteAllTextAsync(FilePath, FileContent);

    var content = await _sut.ReadFromFile(FilePath);
    content.Should().Be(FileContent);
  }

  [Fact]
  public async Task ReadFromFile_ShouldThrowIOException_WhenFileDoesNotExist() {
    await _sut.RemoveFile(FilePath);
    var action = async () => await _sut.ReadFromFile(FilePath);
    await action.Should().ThrowAsync<IOException>();
  }

  [Fact]
  public async Task RemoveFile_ShouldRemoveFile_WhenFileExists() {
    await _sut.CreateFile(FilePath);
    await _sut.RemoveFile(FilePath);

    File.Exists(FilePath).Should().BeFalse();
  }

  [Fact]
  public async Task RemoveFile_ShouldNotThrowException_WhenFileDoesNotExist() {
    var action = async () => await _sut.RemoveFile("BeepBapBoop");
    await action.Should().NotThrowAsync();
  }

  [Fact(Skip = @"
This test takes quite some time to run and is only used to
initially verify that the mutex is working correctly.
")]
  public async Task WriteToFile_ShouldWaitForOtherThread_WhenFileHasAnExistingMutex() {
    var bigFileContent = GetReallyBigString(1_000_000_000);
    var action = async () => { await _sut.WriteToFile(FilePath, bigFileContent); };

    var tasks = new Task[100];

    for (var i = 0; i < tasks.Length; i++) {
      tasks[i] = Task.Run(action);
    }

    try {
      await Task.WhenAll(tasks);
    }
    catch (AggregateException e) {
      Assert.True(false, "All tasks should have completed successfully");
    }

    tasks
      .All(task => task.Exception is null)
      .Should()
      .BeTrue();

    var content = await _sut.ReadFromFile(FilePath);
    content.Length.Should().Be(bigFileContent.Length);
  }

  private string GetReallyBigString(int size = 100) {
    var stringSpan = string.Create(size, "", (span, s) => {
      for (var i = 0; i < size; i++) {
        span[i] = 'a';
      }
    });

    return stringSpan;
  }
}