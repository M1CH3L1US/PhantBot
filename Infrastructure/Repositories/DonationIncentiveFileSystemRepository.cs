using Core.Configuration;
using Core.Entities;
using Core.Repositories;
using Core.Storage;
using Infrastructure.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Repositories;

public class DonationIncentiveFileSystemRepository : IDonationIncentiveRepository {
  private readonly string _filePath;
  private readonly IThreadSafeFileWriter _fileWriter;

  public DonationIncentiveFileSystemRepository(
    IThreadSafeFileWriter fileWriter,
    IOptions<StorageConfiguration> configuration
  ) {
    _fileWriter = fileWriter;
    _filePath = configuration.Value.IncentiveFilePath;
  }

  public async Task<IDonationIncentive> Get() {
    var fileContent = await ReadIncentiveFileContent();
    var incentive = ParseFromFileContent(fileContent);
    return incentive;
  }

  public Task<IDonationIncentive> Set(IDonationIncentive donationIncentive) {
    throw new NotImplementedException();
  }

  public Task<decimal> GetGoal() {
    throw new NotImplementedException();
  }

  public Task<decimal> SetGoal(decimal goal) {
    throw new NotImplementedException();
  }

  public Task<decimal> AddToGoal(decimal amount) {
    throw new NotImplementedException();
  }

  public Task<decimal> RemoveFromGoal(decimal amount) {
    throw new NotImplementedException();
  }

  public Task<decimal> GetAmount() {
    throw new NotImplementedException();
  }

  public Task<decimal> SetAmount(decimal amount) {
    throw new NotImplementedException();
  }

  public Task<decimal> AddAmount(decimal amount) {
    throw new NotImplementedException();
  }

  public Task<decimal> RemoveAmount(decimal amount) {
    throw new NotImplementedException();
  }

  public Task Initialize(IDonationIncentive incentive) {
    throw new NotImplementedException();
  }

  private bool FileExists() {
    return _fileWriter.FileExists(_filePath);
  }

  private IDonationIncentive ParseFromFileContent(string fileContent) {
    if (string.IsNullOrEmpty(fileContent)) {
      return new DonationIncentive();
    }

    var incentive = JsonConvert.DeserializeObject<DonationIncentive>(fileContent);
    return incentive!;
  }

  private string ParseToFileContent(IDonationIncentive incentive) {
    return JsonConvert.SerializeObject(incentive, Formatting.Indented);
  }

  private async Task<string> ReadIncentiveFileContent() {
    if (FileExists()) {
      return await _fileWriter.ReadFromFile(_filePath);
    }

    await _fileWriter.CreateFile(_filePath);
    return "";
  }
}