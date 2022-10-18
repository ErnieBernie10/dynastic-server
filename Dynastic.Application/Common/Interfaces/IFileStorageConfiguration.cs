namespace Dynastic.Application.Common.Interfaces;

public interface IFileStorageConfiguration
{
    public string UserCoaStoragePath { get; set; }
    public string CoaFileServePath { get; set; }

    public string UserCoaEnvironmentPath();
}