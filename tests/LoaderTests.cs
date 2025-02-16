using FluentAssertions;
using FileLoader;

namespace tests;

public class LoaderTests : IDisposable
{
    private readonly Loader _loader;

    public LoaderTests()
    {
        _loader = new Loader("suppliers.txt");
        ResetData();
    }

    private void ResetData()
    {
        var newList = new List<Supplier>
        {
            new() { Id = "1", Address = "Add1", Name = "Supp1" },
            new() { Id = "2", Address = "Add2", Name = "Supp2" },
            new() { Id = "3", Address = "Add3", Name = "Supp3" }
        };

        var field = typeof(Loader).GetField("suppliers",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Static);

        field?.SetValue(null, newList);
    }

    public void Dispose()
    {
        ResetData();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void LoadSupplier_WithValidId_ReturnsSupplier()
    {
        // Arrange
        const string id = "1";

        // Act
        var result = _loader.LoadSupplier(id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Name.Should().Be("Supp1");
        result.Address.Should().Be("Add1");
    }

    [Fact]
    public void LoadSupplier_WithInvalidId_ThrowsApiException()
    {
        // Arrange
        const string id = "nonexistent";

        // Act
        var act = () => _loader.LoadSupplier(id);

        // Assert
        act.Should().Throw<ApiException>();
    }

    [Fact]
    public void LoadSuppliers_ReturnsAllSuppliers()
    {
        // Act
        var result = _loader.LoadSuppliers();

        // Assert
        var enumerable = result as Supplier[] ?? result.ToArray();
        enumerable.Should().HaveCount(3);
        enumerable.Should().Contain(s => s.Id == "1");
        enumerable.Should().Contain(s => s.Id == "2");
        enumerable.Should().Contain(s => s.Id == "3");
    }

    [Fact]
    public void InsertSupplier_WithValidSupplier_AddsToList()
    {
        // Arrange
        var supplier = new Supplier { Id = "4", Name = "Supp4", Address = "Add4" };

        // Act
        _loader.InsertSupplier(supplier);
        var result = _loader.LoadSuppliers();

        // Assert
        result.Should().Contain(s => s.Id == "4");
    }

    [Theory]
    [InlineData("", "Name")]
    [InlineData("Id", "")]
    public void InsertSupplier_WithInvalidData_ThrowsApiException(string id, string name)
    {
        // Arrange
        var supplier = new Supplier { Id = id, Name = name };

        // Act
        var act = () => _loader.InsertSupplier(supplier);

        // Assert
        act.Should().Throw<ApiException>();
    }

    [Fact]
    public void UpdateSupplier_WithValidSupplier_UpdatesExistingSupplier()
    {
        // Arrange
        var supplier = new Supplier { Id = "1", Name = "Updated", Address = "NewAdd" };

        // Act
        _loader.UpdateSupplier(supplier);
        var result = _loader.LoadSupplier("1");

        // Assert
        result.Name.Should().Be("Updated");
        result.Address.Should().Be("NewAdd");
    }

    [Fact]
    public void DeleteSupplier_WithValidId_RemovesSupplier()
    {
        // Arrange
        const string id = "1";

        // Act
        _loader.DeleteSupplier(id);
        var result = _loader.LoadSuppliers();

        // Assert
        result.Should().NotContain(s => s.Id == "1");
    }
}
