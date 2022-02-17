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

  public async Task<IDonationIncentive> GetOrCreate() {
    var fileContent = await ReadIncentiveFileContent();
    var incentive = ParseFromFileContent(fileContent);

    if (!FileExists()) {
      await Set(incentive);
    }

    return incentive;
  }

  public async Task<IDonationIncentive> Set(IDonationIncentive donationIncentive) {
    var content = ParseToFileContent(donationIncentive);
    var previousState = await GetIncentiveFromFile();

    await WriteIncentiveToFile(content);

    return previousState;
  }

  public async Task<decimal> GetGoal() {
    var incentive = await GetIncentiveFromFile();
    return incentive.Goal;
  }

  public async Task<decimal> GetAmount() {
    var incentive = await GetIncentiveFromFile();
    return incentive.Amount;
  }

  public Task<IDonationIncentive> SetGoal(decimal goal) {
    return UpdateIncentive(incentive => incentive.Goal = goal);
  }

  public Task<IDonationIncentive> AddToGoal(decimal amount) {
    return UpdateIncentive(incentive => incentive.Goal += amount);
  }

  public Task<IDonationIncentive> RemoveFromGoal(decimal amount) {
    return UpdateIncentive(incentive => incentive.Goal -= amount);
  }

  public Task<IDonationIncentive> SetAmount(decimal amount) {
    return UpdateIncentive(incentive => incentive.Goal = amount);
  }

  public Task<IDonationIncentive> AddToAmount(decimal amount) {
    return UpdateIncentive(incentive => incentive.Amount += amount);
  }

  public Task<IDonationIncentive> RemoveFromAmount(decimal amount) {
    return UpdateIncentive(incentive => incentive.Amount -= amount);
  }

  private async Task<IDonationIncentive> UpdateIncentive(
    Action<IDonationIncentive> updateAction
  ) {
    var incentive = await GetIncentiveFromFile();
    var previousIncentive = (IDonationIncentive) incentive.Clone();

    updateAction(incentive);
    await Set(incentive);

    return previousIncentive;
  }

  private bool FileExists() {
    return _fileWriter.FileExists(_filePath);
  }

  private async Task WriteIncentiveToFile(string content) {
    await _fileWriter.WriteToFile(_filePath, content);
  }

  private async Task<IDonationIncentive> GetIncentiveFromFile() {
    var fileContent = await ReadIncentiveFileContent();
    return ParseFromFileContent(fileContent);
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

  private async Task<string?> ReadIncentiveFileContent() {
    if (!FileExists()) {
      return null;
    }

    return await _fileWriter.ReadFromFile(_filePath)!;
  }
}