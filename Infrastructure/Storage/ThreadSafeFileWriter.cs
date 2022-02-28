using System.IO.Abstractions;
using Core.Storage;

namespace Infrastructure.Storage;

public class ThreadSafeFileWriter : IThreadSafeFileWriter {
  private readonly IFileSystem _fileSystem;
  private readonly TimeSpan _writeTimeout;

  public ThreadSafeFileWriter(IFileSystem fileSystem, TimeSpan? writeTimeout = null) {
    _writeTimeout = writeTimeout ?? TimeSpan.FromSeconds(10);
    _fileSystem = fileSystem;
  }

  public IFile File => _fileSystem.File;
  public IDirectory Directory => _fileSystem.Directory;

  public Task CreateFile(string path) {
    if (FileExists(path)) {
      return Task.CompletedTask;
    }

    _fileSystem.File.Create(path);
    return Task.CompletedTask;
  }

  public Task<string>? ReadFromFile(string path) {
    if (!FileExists(path)) {
      throw new IOException($"File {path} does not exist");
    }

    return _fileSystem.File.ReadAllTextAsync(path);
  }

  public Task WriteToFile(string path, string content) {
    if (!FileDirectoryExists(path)) {
      CreateFileDirectory(path);
    }

    var action = () => File.WriteAllText(path, content);
    return CallActionInMutex(path, action);
  }

  public Task RemoveFile(string path) {
    if (!FileExists(path)) {
      return Task.CompletedTask;
    }

    return CallActionInMutex(path, () => File.Delete(path));
  }

  public bool FileExists(string path) {
    return _fileSystem.File.Exists(path);
  }

  public Task CallActionInMutex(string path, Action action) {
    return Task.Run(() => {
      var mutex = GetMutex(path);
      var didAcquireMutex = false;

      try {
        didAcquireMutex = mutex.WaitOne(_writeTimeout, false);
        if (didAcquireMutex) {
          action();
        }
      }
      finally {
        if (didAcquireMutex) {
          ReleaseMutex(mutex);
        }
        else {
          throw new TimeoutException($"Timout reached while trying to acquire access to file \"${path}\"");
        }
      }
    });
  }

  private bool FileDirectoryExists(string path) {
    var directory = GetFileDirectory(path);
    return Directory.Exists(directory.FullName);
  }

  private void CreateFileDirectory(string path) {
    if (FileDirectoryExists(path)) {
      return;
    }

    var directory = GetFileDirectory(path);
    Directory.CreateDirectory(directory.FullName);
  }

  private DirectoryInfo GetFileDirectory(string path) {
    var fullPath = Path.GetFullPath(path);
    var fileInfo = new FileInfo(fullPath);

    return fileInfo.Directory;
  }

  private Mutex GetMutex(string path) {
    var id = GetMutexId(path);
    var mutex = new Mutex(false, id);

    return mutex;
  }

  private void ReleaseMutex(Mutex mutex) {
    mutex.ReleaseMutex();
  }

  private string GetMutexId(string filePath) {
    var filePathWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
    // https://docs.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=net-6.0#remarks
    return $"Global\\{nameof(ThreadSafeFileWriter)}-{filePathWithoutExtension}";
  }
}