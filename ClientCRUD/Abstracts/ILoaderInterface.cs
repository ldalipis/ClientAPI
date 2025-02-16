using FileLoader;

namespace ClientCRUD.Abstracts;

public interface ILoader
{
    Task InsertSupplier(Supplier supplier);
    Task<IEnumerable<Supplier>> LoadSuppliers();
    Task<Supplier> LoadSupplier(string id);
    Task UpdateSupplier(Supplier supplier);
    Task DeleteSupplier(string id);
}
