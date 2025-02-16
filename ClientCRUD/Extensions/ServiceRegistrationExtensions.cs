using ClientCRUD.Abstracts;
using ClientCRUD.Configurations;
using ClientCRUD.Loaders;

namespace ClientCRUD.Extensions;

public static class ServiceRegistration
{
    public static void AddLoaderServices(this IServiceCollection services, LoaderSettings loaderSettings)
    {
        services.AddScoped<ILoader>(_ => new LoaderWrapper(loaderSettings.LocalFile ?? string.Empty));
        services.AddScoped<IDataLoader>(_ => new DataLoaderWrapper(
            loaderSettings.Server ?? string.Empty,
            loaderSettings.UserId ?? string.Empty,
            loaderSettings.Password ?? string.Empty));

        services.AddScoped<LoaderAdapter>();
        services.AddScoped<DataLoaderAdapter>();

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
