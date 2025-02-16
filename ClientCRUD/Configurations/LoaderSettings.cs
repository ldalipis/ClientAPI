using Microsoft.Extensions.Options;

namespace ClientCRUD.Configurations
{
    public class LoaderSettings
    {
        public string? LocalFile { get; init; }
        public string? Server { get; init; }
        public string? UserId { get; init; }
        public string? Password { get; init; }
        public string? LoaderType { get; init; }
    }

    public class LoaderSettingsValidation : IValidateOptions<LoaderSettings>
    {
        public ValidateOptionsResult Validate(string? name, LoaderSettings settings)
        {
            if (settings.LoaderType != "File" && settings.LoaderType != "SqlServer")
                return ValidateOptionsResult.Fail("LoaderType must be either 'File' or 'SqlServer'.");

            if (string.IsNullOrEmpty(settings.LocalFile) && settings.LoaderType == "File")
                return ValidateOptionsResult.Fail("LocalFile is required for File loader.");

            if ((string.IsNullOrEmpty(settings.Server) || string.IsNullOrEmpty(settings.UserId) ||
                 string.IsNullOrEmpty(settings.Password)) && settings.LoaderType == "SqlServer")
                return ValidateOptionsResult.Fail("Server, UserId, and Password are required for SqlServer loader.");

            return ValidateOptionsResult.Success;
        }
    }
}
