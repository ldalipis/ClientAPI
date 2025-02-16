using ClientCRUD.Abstracts;
using ClientCRUD.Loaders;
using ClientCRUD.Models;
using Moq;
using SqlServerLoader;
using FluentAssertions;

namespace tests;

public class DataLoaderAdapterTests
{
    private readonly Mock<IDataLoader> _mockDataLoader;
    private readonly DataLoaderAdapter _sut;

    public DataLoaderAdapterTests()
    {
        _mockDataLoader = new Mock<IDataLoader>();
        _sut = new DataLoaderAdapter(_mockDataLoader.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldCallInsertTrader_WithCorrectData()
    {
        // Arrange
        var request = new UnifiedRequestModel
        {
            Id = "TEST001",
            Description = "Test Trader",
            Address = "123 Test Street"
        };

        // Act
        await _sut.AddAsync(request);

        // Assert
        _mockDataLoader.Verify(x => x.InsertTrader(It.Is<Trader>(t =>
            t.Code == request.Id &&
            t.Description == request.Description &&
            t.Street == request.Address)), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedTraders()
    {
        // Arrange
        var traders = new List<Trader>
        {
            new() { Code = "T1", Description = "Trader 1", Street = "Street 1" },
            new() { Code = "T2", Description = "Trader 2", Street = "Street 2" }
        };

        _mockDataLoader.Setup(x => x.LoadTraders())
            .ReturnsAsync(traders);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(2);
        resultList.Should().AllBeOfType<UnifiedResponseModel>();
        resultList.Should().AllSatisfy(r => r.Source.Should().Be("Database"));

        resultList[0].Id.Should().Be("T1");
        resultList[0].Description.Should().Be("Trader 1");
        resultList[0].Address.Should().Be("Street 1");

        resultList[1].Id.Should().Be("T2");
        resultList[1].Description.Should().Be("Trader 2");
        resultList[1].Address.Should().Be("Street 2");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMappedTrader()
    {
        // Arrange
        var trader = new Trader
        {
            Code = "T1",
            Description = "Trader 1",
            Street = "Street 1"
        };

        _mockDataLoader.Setup(x => x.LoadTrader("T1"))
            .ReturnsAsync(trader);

        // Act
        var result = await _sut.GetByIdAsync("T1");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("T1");
        result.Description.Should().Be("Trader 1");
        result.Address.Should().Be("Street 1");
        result.Source.Should().Be("Database");
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallUpdateTrader_WithCorrectData()
    {
        // Arrange
        var request = new UnifiedRequestModel
        {
            Id = "TEST001",
            Description = "Updated Trader",
            Address = "456 Updated Street"
        };

        // Act
        await _sut.UpdateAsync(request);

        // Assert
        _mockDataLoader.Verify(x => x.UpdateTrader(It.Is<Trader>(t =>
            t.Code == request.Id &&
            t.Description == request.Description &&
            t.Street == request.Address)), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallDeleteTrader_WithCorrectId()
    {
        // Arrange
        const string id = "TEST001";

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _mockDataLoader.Verify(x => x.DeleteTrader(id), Times.Once);
    }
}
