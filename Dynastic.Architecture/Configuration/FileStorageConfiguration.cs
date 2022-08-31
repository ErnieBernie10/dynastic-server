using Dynastic.Application.Common.Interfaces;

namespace Dynastic.Infrastructure.Configuration;

public class FileStorageConfiguration : IFileStorageConfiguration
{
    private readonly bool _isDevelopment;

    public FileStorageConfiguration(bool isDevelopment)
    {
        this._isDevelopment = isDevelopment;
    }

    public string UserCoaStoragePath { get; set; }

    public string UserCoaEnvironmentPath()
    {
        var devPath = Path.Combine(Directory.GetCurrentDirectory(), UserCoaStoragePath);
        if (!Directory.Exists(devPath))
        {
            Directory.CreateDirectory(devPath);
        }

        return _isDevelopment
            ? devPath
            : UserCoaStoragePath;
    }
}