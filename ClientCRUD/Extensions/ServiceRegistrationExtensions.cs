using ClientCRUD.Abstracts;
using ClientCRUD.Configurations;
using ClientCRUD.Loaders;
using FileLoader;
using SqlServerLoader;

namespace ClientCRUD.Extensions;

public static class ServiceRegistration
{
    public static void AddLoaderServices(this IServiceCollection services, LoaderSettings loaderSettings)
    {
        services.AddScoped<LoaderAdapter>(_ => new LoaderAdapter(new Loader(loaderSettings.LocalFile)));
        services.AddScoped<DataLoaderAdapter>(_ => new DataLoaderAdapter(new DataLoader(
            loaderSettings.Server,
            loaderSettings.UserId,
            loaderSettings.Password)));

        services.AddScoped<IResourceLoader>(sp =>
        {
            return loaderSettings.LoaderType switch
            {
                "File" => sp.GetRequiredService<LoaderAdapter>(),
                "SqlServer" => sp.GetRequiredService<DataLoaderAdapter>(),
                _ => throw new InvalidOperationException(
                    $"Invalid LoaderType: {loaderSettings.LoaderType}. Supported values are 'File' or 'SqlServer'.")
            };
        });
    }
}
