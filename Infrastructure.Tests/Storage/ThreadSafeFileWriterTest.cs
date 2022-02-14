using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
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
    var timeout = TimeSpan.FromSeconds(2);
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
  public async Task WriteToFile_ShouldWaitForOtherThread_WhenFileHasAnExistingMutex() {
    var action = async () => { await _sut.WriteToFile(FilePath, FileContent); };

    var thread1 = new Thread(async () => {
      _sut._fileLocks.Any().Should().BeTrue();
      await action();
    });
    var thread2 = new Thread(async () => {
      _sut._fileLocks.Any().Should().BeTrue();
      await action();
    });
    var thread3 = new Thread(async () => {
      _sut._fileLocks.Any().Should().BeTrue();
      await action();
    });

    thread1.Start();
    thread2.Start();
    thread3.Start();
  }

  // _sut.Write(FilePath, FileContent);
  // var file = FileInfo(FilePath);

  // Assert.True(file.Exists);

  // Assert.Equal(FileContent, file.OpenText().ReadToEnd());
}