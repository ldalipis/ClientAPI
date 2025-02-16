using FileLoader;

namespace ClientCRUD.Abstracts;

public class LoaderWrapper(string filePath) : ILoader
{
    private readonly Loader _loader = new(filePath);

    public Task InsertSupplier(Supplier supplier)
    {
        _loader.InsertSupplier(supplier);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Supplier>> LoadSuppliers() => Task.FromResult(_loader.LoadSuppliers());
    public Task<Supplier> LoadSupplier(string id) => Task.FromResult(_loader.LoadSupplier(id));

    public Task UpdateSupplier(Supplier supplier)
    {
        _loader.UpdateSupplier(supplier);
        return Task.CompletedTask;
    }

    public Task DeleteSupplier(string id)
    {
        _loader.DeleteSupplier(id);
        return Task.CompletedTask;
    }
}
