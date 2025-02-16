using FluentAssertions;
using SqlServerLoader;

namespace tests;

public class DataLoaderTests : IDisposable
{
    private readonly DataLoader _dataLoader;

    public DataLoaderTests()
    {
        _dataLoader = new DataLoader("server", "userid", "password");
        ResetData();
    }

    private static void ResetData()
    {
        var list = new List<Trader>
        {
            new() { Code = "sql1", Street = "sqlAdd1", Description = "sqlSupp1" },
            new() { Code = "sql2", Street = "sqlAdd2", Description = "sqlSupp2" }
        };

        // We need to use reflection to reset the static list since it's private
        var field = typeof(DataLoader).GetField("traders",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Static);

        field?.SetValue(null, list);
    }

    public void Dispose()
    {
        ResetData();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task LoadTrader_WithValidCode_ReturnsTrader()
    {
        // Arrange
        const string code = "sql1";

        // Act
        var result = await _dataLoader.LoadTrader(code);

        // Assert
        result.Should().NotBeNull();
        result.Code.Should().Be("sql1");
        result.Description.Should().Be("sqlSupp1");
        result.Street.Should().Be("sqlAdd1");
    }

    [Fact]
    public async Task LoadTrader_WithInvalidCode_ThrowsException()
    {
        // Arrange
        const string code = "nonexistent";

        // Act
        var act = () => _dataLoader.LoadTrader(code);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Trader not found");
    }

    [Fact]
    public async Task LoadTraders_ReturnsAllTraders()
    {
        // Act
        var result = await _dataLoader.LoadTraders();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(t => t.Code == "sql1");
        result.Should().Contain(t => t.Code == "sql2");
    }

    [Fact]
    public async Task InsertTrader_WithValidTrader_AddsToList()
    {
        // Arrange
        var trader = new Trader
        {
            Code = "sql3",
            Description = "sqlSupp3",
            Street = "sqlAdd3"
        };

        // Act
        await _dataLoader.InsertTrader(trader);
        var result = await _dataLoader.LoadTraders();

        // Assert
        result.Should().Contain(t => t.Code == "sql3");
    }

    [Theory]
    [InlineData("", "Description")]
    [InlineData("Code", "")]
    public async Task InsertTrader_WithInvalidData_ThrowsException(string code, string description)
    {
        // Arrange
        var trader = new Trader { Code = code, Description = description };

        // Act
        var act = () => _dataLoader.InsertTrader(trader);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Code and description are required");
    }

    [Fact]
    public async Task UpdateTrader_WithValidTrader_UpdatesExistingTrader()
    {
        // Arrange
        var trader = new Trader
        {
            Code = "sql1",
            Description = "Updated",
            Street = "NewStreet"
        };

        // Act
        await _dataLoader.UpdateTrader(trader);
        var result = await _dataLoader.LoadTrader("sql1");

        // Assert
        result.Description.Should().Be("Updated");
        result.Street.Should().Be("NewStreet");
    }

    [Fact]
    public async Task DeleteTrader_WithValidCode_RemovesTrader()
    {
        // Arrange
        const string code = "sql1";

        // Act
        await _dataLoader.DeleteTrader(code);
        var result = await _dataLoader.LoadTraders();

        // Assert
        result.Should().NotContain(t => t.Code == "sql1");
    }

    [Fact]
    public void Constructor_WithInvalidConnectionInfo_ThrowsException()
    {
        // Arrange & Act
        var act = () => new DataLoader("wrongServer", "wrongUser", "wrongPass")
            .LoadTraders();

        // Assert
        act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Wrong connection info");
    }
}
