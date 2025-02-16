using Microsoft.Extensions.Options;

namespace ClientCRUD.Configurations
{
    public class LoaderSettings
    {
        public string? LocalFile { get; set; }
        public string? Server { get; set; }
        public string? UserId { get; set; }
        public string? Password { get; set; }
        public string? LoaderType { get; set; }
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
