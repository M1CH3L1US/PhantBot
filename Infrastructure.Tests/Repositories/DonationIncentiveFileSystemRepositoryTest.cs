using System;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Entities;
using FluentAssertions;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class DonationIncentiveFileSystemRepositoryTest {
  private readonly IFileSystem _fileSystem;
  private readonly StorageConfiguration _storageConfiguration;
  private readonly DonationIncentiveFileSystemRepository _sut;


  public DonationIncentiveFileSystemRepositoryTest(
    IFileSystem fileSystem,
    IOptions<StorageConfiguration> storageConfiguration) {
    var fileWriter = new ThreadSafeFileWriter(fileSystem, TimeSpan.FromSeconds(5));

    _fileSystem = fileSystem;
    _storageConfiguration = storageConfiguration.Value;
    _sut = new DonationIncentiveFileSystemRepository(
      fileWriter,
      storageConfiguration
    );
  }

  [Fact]
  public async Task GetOrCreate_ShouldReturnEmptyDonationIncentive_WhenFileDoesNotExist() {
    RemoveIncentiveFile();
    var incentive = await _sut.GetOrCreate();

    incentive.Amount.Should().Be(0);
    incentive.Goal.Should().Be(0);
  }

  [Fact]
  public async Task GetOrCreate_CreatesFile_WhenFileDoesNotExist() {
    RemoveIncentiveFile();
    await _sut.GetOrCreate();

    var fileContent = GetIncentiveFromFileSystem();
    fileContent.Should().NotBeNull();
  }

  [Fact]
  public async Task GetOrCreate_ShouldReturnCurrentIncentive_WhenFileContainsIncentive() {
    MakeAndWriteIncentive();

    var currentIncentive = await _sut.GetOrCreate();

    currentIncentive.Amount.Should().Be(100);
    currentIncentive.Goal.Should().Be(100);
  }

  [Fact]
  public async Task Set_ShouldWriteIncentiveToFile_WhenCalled() {
    var incentive = new DonationIncentive {
      Amount = 100,
      Goal = 100
    };

    await _sut.Set(incentive);
    var fsIncentive = GetIncentiveFromFileSystem()!;

    fsIncentive.Should().NotBeNull();
    fsIncentive.Amount.Should().Be(100);
    fsIncentive.Goal.Should().Be(100);
  }

  [Fact]
  public async Task Set_ShouldOverwriteExistingIncentive_WhenCalled() {
    MakeAndWriteIncentive();

    var newIncentive = new DonationIncentive {
      Amount = 200,
      Goal = 200
    };

    await _sut.Set(newIncentive);
    var fsIncentive = GetIncentiveFromFileSystem()!;

    fsIncentive.Should().NotBeNull();
    fsIncentive.Amount.Should().Be(200);
    fsIncentive.Goal.Should().Be(200);
  }

  [Fact]
  public async Task Set_ShouldReturnPreviousState_WhenCalled() {
    var incentive = MakeAndWriteIncentive();

    var newIncentive = new DonationIncentive {
      Amount = 200,
      Goal = 200
    };

    var previousIncentive = await _sut.Set(newIncentive);

    previousIncentive.Amount.Should().Be(incentive.Amount);
    previousIncentive.Goal.Should().Be(incentive.Goal);
  }

  [Fact]
  public async Task GetGoal_ShouldReturnCurrentGoal_WhenFileExists() {
    var currentGoal = await _sut.GetGoal();

    currentGoal.Should().Be(100);
  }

  [Fact]
  public async Task GetGoal_ShouldReturnZero_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    var currentGoal = await _sut.GetGoal();

    currentGoal.Should().Be(0);
  }

  [Fact]
  public async Task GetAmount_ShouldReturnCurrentAmount_WhenFileExists() {
    var incentive = MakeAndWriteIncentive();

    var currentAmount = await _sut.GetAmount();

    currentAmount.Should().Be(incentive.Amount);
  }

  [Fact]
  public async Task GetAmount_ShouldReturnZero_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    var currentAmount = await _sut.GetAmount();

    currentAmount.Should().Be(0);
  }

  [Fact]
  public async Task AddToGoal_ShouldAddToCurrentGoal_WhenFileExists() {
    var incentive = MakeAndWriteIncentive();

    await _sut.AddToGoal(10);

    var currentGoal = await _sut.GetGoal();

    currentGoal.Should().Be(incentive.Goal + 10);
  }

  [Fact]
  public async Task AddToGoal_ShouldCreateFile_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    await _sut.AddToGoal(10);
    var currentGoal = await _sut.GetGoal();
    var incentive = GetIncentiveFromFileSystem();

    currentGoal.Should().Be(10);
    incentive.Should().NotBeNull();
  }

  [Fact]
  public async Task AddToGoal_ShouldReturnPreviousGoal_WhenCalled() {
    var incentive = MakeAndWriteIncentive();

    var previousGoal = await _sut.AddToGoal(10);

    IsSameIncentive(previousGoal, incentive).Should().BeTrue();
  }

  [Fact]
  public async Task AddToAmount_ShouldAddToCurrentAmount_WhenFileExists() {
    var incentive = MakeAndWriteIncentive();

    await _sut.AddToAmount(10);

    var currentAmount = await _sut.GetAmount();

    currentAmount.Should().Be(incentive.Amount + 10);
  }

  [Fact]
  public async Task AddToAmount_ShouldCreateFile_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    await _sut.AddToAmount(10);
    var currentAmount = await _sut.GetAmount();
    var incentive = GetIncentiveFromFileSystem();

    currentAmount.Should().Be(10);
    incentive.Should().NotBeNull();
  }

  [Fact]
  public async Task AddToAmount_ShouldReturnPreviousAmount_WhenCalled() {
    var incentive = MakeAndWriteIncentive();

    var previousAmount = await _sut.AddToAmount(10);

    IsSameIncentive(previousAmount, incentive).Should().BeTrue();
  }

  [Fact]
  public async Task RemoveFromGoal_ShouldRemoveFromCurrentGoal_WhenFileExists() {
    var incentive = MakeAndWriteIncentive();

    await _sut.RemoveFromGoal(10);

    var currentGoal = await _sut.GetGoal();

    currentGoal.Should().Be(incentive.Goal - 10);
  }

  [Fact]
  public async Task RemoveFromGoal_ShouldCreateFile_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    await _sut.RemoveFromGoal(10);
    var currentGoal = await _sut.GetGoal();
    var incentive = GetIncentiveFromFileSystem();

    currentGoal.Should().Be(-10);
    incentive.Should().NotBeNull();
  }

  [Fact]
  public async Task RemoveFromGoal_ShouldReturnPreviousGoal_WhenCalled() {
    var incentive = MakeAndWriteIncentive();

    var previousGoal = await _sut.RemoveFromGoal(10);

    IsSameIncentive(previousGoal, incentive).Should().BeTrue();
  }

  [Fact]
  public async Task RemoveFromAmount_ShouldRemoveFromCurrentAmount_WhenFileExists() {
    var incentive = MakeAndWriteIncentive();

    await _sut.RemoveFromAmount(10);

    var currentAmount = await _sut.GetAmount();

    currentAmount.Should().Be(incentive.Amount - 10);
  }

  [Fact]
  public async Task RemoveFromAmount_ShouldCreateFile_WhenFileDoesNotExist() {
    RemoveIncentiveFile();

    await _sut.RemoveFromAmount(10);
    var currentAmount = await _sut.GetAmount();
    var incentive = GetIncentiveFromFileSystem();

    currentAmount.Should().Be(-10);
    incentive.Should().NotBeNull();
  }

  [Fact]
  public async Task RemoveFromAmount_ShouldReturnPreviousAmount_WhenCalled() {
    var incentive = MakeAndWriteIncentive();

    var previousAmount = await _sut.RemoveFromAmount(10);

    IsSameIncentive(previousAmount, incentive).Should().BeTrue();
  }

  private static bool IsSameIncentive(IDonationIncentive comparable, IDonationIncentive other) {
    return comparable.Amount == other.Amount &&
           comparable.Goal == other.Goal;
  }

  private IDonationIncentive MakeAndWriteIncentive() {
    var incentive = new DonationIncentive {
      Amount = 100,
      Goal = 100
    };
    WriteIncentiveToFile(incentive);
    return incentive;
  }


  private void RemoveIncentiveFile() {
    var filePath = GetIncentiveFilePath();
    _fileSystem.File.Delete(filePath);
  }

  private void WriteIncentiveToFile(IDonationIncentive incentive) {
    var filePath = GetIncentiveFilePath();
    var content = JsonConvert.SerializeObject(incentive);
    _fileSystem.File.WriteAllText(filePath, content);
  }

  private string GetIncentiveFilePath() {
    return _storageConfiguration.IncentiveFilePath;
  }

  private IDonationIncentive? GetIncentiveFromFileSystem() {
    var filePath = GetIncentiveFilePath();
    var content = _fileSystem.File.ReadAllText(filePath);

    if (string.IsNullOrWhiteSpace(content)) {
      return null;
    }

    return JsonConvert.DeserializeObject<DonationIncentive>(content);
  }
}