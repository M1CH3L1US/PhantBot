using System.IO.Abstractions;
using System.Threading.Tasks;
using Core.Configuration;
using Core.Entities;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;

namespace Infrastructure.Tests.Repositories;

public class DonationIncentiveFileSystemRepositoryTest {
  private readonly IFileSystem _fileSystem;
  private readonly DonationIncentiveFileSystemRepository _sut;

  public DonationIncentiveFileSystemRepositoryTest(
    IFileSystem fileSystem,
    IOptions<StorageConfiguration> storageConfiguration
  ) {
    _fileSystem = fileSystem;
  }

  [Fact]
  public async Task Get_ShouldReturnEmptyDonationIncentive_WhenFileDoesNotExist() {
    // var fileSystem = new DonationIncentiveFileSystemRepository(_fileSystem);
    // var result = await fileSystem.Get();
    //
    // Assert.Empty(result);
  }

  private IDonationIncentive GetDataFromFileSystem() {
    var file = _fileSystem.File.ReadAllText("");
    return JsonConvert.DeserializeObject<IDonationIncentive>(file);
  }
}