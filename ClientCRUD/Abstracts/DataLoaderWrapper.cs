using SqlServerLoader;

namespace ClientCRUD.Abstracts;

public class DataLoaderWrapper(string server, string userId, string password) : IDataLoader
{
    private readonly DataLoader _dataLoader = new(server, userId, password);

    public Task InsertTrader(Trader trader) => _dataLoader.InsertTrader(trader);
    public Task<List<Trader>> LoadTraders() => _dataLoader.LoadTraders();
    public Task<Trader> LoadTrader(string id) => _dataLoader.LoadTrader(id);
    public Task UpdateTrader(Trader trader) => _dataLoader.UpdateTrader(trader);
    public Task DeleteTrader(string id) => _dataLoader.DeleteTrader(id);
}
