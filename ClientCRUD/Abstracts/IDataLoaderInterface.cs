using SqlServerLoader;

namespace ClientCRUD.Abstracts;

public interface IDataLoader
{
    Task InsertTrader(Trader trader);
    Task<List<Trader>> LoadTraders();
    Task<Trader> LoadTrader(string id);
    Task UpdateTrader(Trader trader);
    Task DeleteTrader(string id);
}
