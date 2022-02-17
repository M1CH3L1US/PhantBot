namespace Core.Storage;

public interface IThreadSafeFileWriter {
  public Task CreateFile(string path);
  public Task<string>? ReadFromFile(string path);
  public Task WriteToFile(string path, string content);
  public Task RemoveFile(string path);
  public bool FileExists(string path);
}