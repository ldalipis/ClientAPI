using ClientCRUD.Abstracts;
using ClientCRUD.Models;
using FileLoader;

namespace ClientCRUD.Loaders;

public class LoaderAdapter(Loader loader) : IResourceLoader
{
    public Task AddAsync(IUnifiedRequestModel request, CancellationToken cancellationToken = default)
    {
        var supplier = new Supplier
        {
            Id = request.Id,
            Name = request.Description,
            Address = request.Address
        };
        loader.InsertSupplier(supplier);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<IUnifiedResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var suppliers = loader.LoadSuppliers();
        var response = suppliers.Select(s => new UnifiedResponseModel
        {
            Id = s.Id,
            Description = s.Name,
            Address = s.Address,
            Source = "File"
        });
        return Task.FromResult<IEnumerable<IUnifiedResponseModel>>(response);
    }

    public Task<IUnifiedResponseModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var supplier = loader.LoadSupplier(id);
        var response = new UnifiedResponseModel
        {
            Id = supplier.Id,
            Description = supplier.Name,
            Address = supplier.Address,
            Source = "File"
        };
        return Task.FromResult<IUnifiedResponseModel>(response);
    }

    public Task UpdateAsync(IUnifiedRequestModel request, CancellationToken cancellationToken = default)
    {
        var supplier = new Supplier
        {
            Id = request.Id,
            Name = request.Description,
            Address = request.Address
        };
        loader.UpdateSupplier(supplier);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        loader.DeleteSupplier(id);
        return Task.CompletedTask;
    }
}
