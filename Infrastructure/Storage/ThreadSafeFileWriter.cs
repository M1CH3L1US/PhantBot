using System.IO.Abstractions;
using Core.Storage;

namespace Infrastructure.Storage;

public class ThreadSafeFileWriter : IThreadSafeFileWriter {
  private readonly IFileSystem _fileSystem;
  private readonly TimeSpan _writeTimeout;

  internal IDictionary<string, Mutex> _fileLocks = new Dictionary<string, Mutex>();

  public ThreadSafeFileWriter(IFileSystem fileSystem, TimeSpan writeTimeout) {
    _writeTimeout = writeTimeout;
    _fileSystem = fileSystem;
  }

  public IFile File => _fileSystem.File;

  public Task CreateFile(string path) {
    if (FileExists(path)) {
      return Task.CompletedTask;
    }

    _fileSystem.File.Create(path);
    return Task.CompletedTask;
  }

  public Task<string> ReadFromFile(string path) {
    throw new NotImplementedException();
  }

  public Task WriteToFile(string path, string content) {
    return Task.Factory.StartNew(() => {
      using var mutex = GetMutex(path);
      var canWriteFile = false;

      try {
        canWriteFile = mutex.WaitOne(_writeTimeout, false);
        File.WriteAllTextAsync(path, content);
      }
      finally {
        if (canWriteFile) {
          ReleaseMutex(mutex);
        }
      }
    });
  }

  public bool FileExists(string path) {
    return _fileSystem.File.Exists(path);
  }

  private Mutex GetMutex(string path) {
    var id = GetMutexId(path);
    var mutex = new Mutex(false, id);
    _fileLocks.TryAdd(path, mutex);

    return mutex;
  }

  private void ReleaseMutex(Mutex mutex) {
    var mutexKey = _fileLocks.FirstOrDefault(x => x.Value == mutex);
    mutex.ReleaseMutex();
    _fileLocks.Remove(mutexKey);
  }

  private string GetMutexId(string filePath) {
    var filePathWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
    // https://docs.microsoft.com/en-us/dotnet/api/system.threading.mutex?view=net-6.0#remarks
    return $"Global\\{nameof(ThreadSafeFileWriter)}-{filePathWithoutExtension}";
  }
}