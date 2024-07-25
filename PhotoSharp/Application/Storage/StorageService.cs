using Application.Settings;
using Microsoft.Extensions.Options;

namespace Application.Storage;

public class StorageService(IOptions<SettingsOptions> settingsOptions) : IStorageService
{
    public void EnsureStorageFoldersExistsAsync()
    {
        var rootFolder = settingsOptions.Value.RootFolder;
        if (!Directory.Exists(rootFolder))
        {
            Directory.CreateDirectory(rootFolder);
        }

        var imagesFolder = Path.Combine(rootFolder, "thumbnails");
        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }
    }
}
