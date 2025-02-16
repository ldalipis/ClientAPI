using ClientCRUD.Abstracts;
using ClientCRUD.Models;
using SqlServerLoader;

namespace ClientCRUD.Loaders;

public class DataLoaderAdapter(DataLoader dataLoader) : IResourceLoader
{
    public async Task AddAsync(UnifiedRequestModel request, CancellationToken cancellationToken = default)
    {
        var trader = new Trader
        {
            Code = request.Id,
            Description = request.Description,
            Street = request.Address
        };
        await dataLoader.InsertTrader(trader);
    }

    public async Task<IEnumerable<UnifiedResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var traders = await dataLoader.LoadTraders();
        var response = traders.Select(t => new UnifiedResponseModel
        {
            Id = t.Code,
            Description = t.Description,
            Address = t.Street,
            Source = "Database"
        });
        return response;
    }

    public async Task<UnifiedResponseModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var trader = await dataLoader.LoadTrader(id);

        var response = new UnifiedResponseModel
        {
            Id = trader.Code,
            Description = trader.Description,
            Address = trader.Street,
            Source = "Database"
        };
        return response;
    }

    public async Task UpdateAsync(UnifiedRequestModel request, CancellationToken cancellationToken = default)
    {
        var trader = new Trader
        {
            Code = request.Id,
            Description = request.Description,
            Street = request.Address
        };
        await dataLoader.UpdateTrader(trader);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await dataLoader.DeleteTrader(id);
    }
}
