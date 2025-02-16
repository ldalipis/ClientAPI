using ClientCRUD.Abstracts;
using ClientCRUD.Models;
using SqlServerLoader;

namespace ClientCRUD.Loaders;

public class DataLoaderAdapter(IDataLoader dataLoader) : IResourceLoader
{
    public async Task AddAsync(IUnifiedRequestModel request, CancellationToken cancellationToken = default)
    {
        var trader = new Trader
        {
            Code = request.Id,
            Description = request.Description,
            Street = request.Address
        };
        await dataLoader.InsertTrader(trader);
    }

    public async Task<IEnumerable<IUnifiedResponseModel>> GetAllAsync(CancellationToken cancellationToken = default)
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

    public async Task<IUnifiedResponseModel> GetByIdAsync(string id, CancellationToken cancellationToken = default)
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

    public async Task UpdateAsync(IUnifiedRequestModel request, CancellationToken cancellationToken = default)
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
