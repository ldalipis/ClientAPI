using ClientCRUD.Abstracts;
using ClientCRUD.Loaders;
using ClientCRUD.Models;
using FileLoader;
using Moq;

namespace ClientCRUD.Tests.Loaders
{
    public class LoaderAdapterTests
    {
        private readonly Mock<ILoader> _loaderMock;
        private readonly LoaderAdapter _sut;

        public LoaderAdapterTests()
        {
            _loaderMock = new Mock<ILoader>();
            _sut = new LoaderAdapter(_loaderMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldCallInsertSupplier_WithCorrectData()
        {
            // Arrange
            var request = new UnifiedRequestModel
            {
                Id = "test-id",
                Description = "Test Supplier",
                Address = "Test Address"
            };

            // Act
            await _sut.AddAsync(request);

            // Assert
            _loaderMock.Verify(x => x.InsertSupplier(It.Is<Supplier>(s =>
                s.Id == request.Id &&
                s.Name == request.Description &&
                s.Address == request.Address)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedSuppliers()
        {
            // Arrange
            var suppliers = new List<Supplier>
            {
                new() { Id = "1", Name = "Supplier 1", Address = "Address 1" },
                new() { Id = "2", Name = "Supplier 2", Address = "Address 2" }
            };

            _loaderMock.Setup(x => x.LoadSuppliers())
                .ReturnsAsync(suppliers);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            var responseList = result.ToList();
            Assert.Equal(2, responseList.Count);
            Assert.All(responseList, r => Assert.Equal("File", r.Source));
            Assert.Collection(responseList,
                item =>
                {
                    Assert.Equal("1", item.Id);
                    Assert.Equal("Supplier 1", item.Description);
                    Assert.Equal("Address 1", item.Address);
                },
                item =>
                {
                    Assert.Equal("2", item.Id);
                    Assert.Equal("Supplier 2", item.Description);
                    Assert.Equal("Address 2", item.Address);
                });
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Id = "test-id",
                Name = "Test Supplier",
                Address = "Test Address"
            };

            _loaderMock.Setup(x => x.LoadSupplier("test-id"))
                .ReturnsAsync(supplier);

            // Act
            var result = await _sut.GetByIdAsync("test-id");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("test-id", result.Id);
            Assert.Equal("Test Supplier", result.Description);
            Assert.Equal("Test Address", result.Address);
            Assert.Equal("File", result.Source);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallUpdateSupplier_WithCorrectData()
        {
            // Arrange
            var request = new UnifiedRequestModel
            {
                Id = "test-id",
                Description = "Updated Supplier",
                Address = "Updated Address"
            };

            // Act
            await _sut.UpdateAsync(request);

            // Assert
            _loaderMock.Verify(x => x.UpdateSupplier(It.Is<Supplier>(s =>
                s.Id == request.Id &&
                s.Name == request.Description &&
                s.Address == request.Address)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDeleteSupplier_WithCorrectId()
        {
            // Arrange
            var id = "test-id";

            // Act
            await _sut.DeleteAsync(id);

            // Assert
            _loaderMock.Verify(x => x.DeleteSupplier(id), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenLoaderReturnsEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            _loaderMock.Setup(x => x.LoadSuppliers())
                .ReturnsAsync(new List<Supplier>());

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_WhenSupplierNotFound_ShouldReturnNull()
        {
            // Arrange
            _loaderMock.Setup(x => x.LoadSupplier(It.IsAny<string>()))
                .ReturnsAsync((Supplier)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() =>
                _sut.GetByIdAsync("non-existent-id"));
        }
    }
}
