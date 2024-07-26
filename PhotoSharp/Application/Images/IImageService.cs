using EF.Models.Models;
using Models.Models;

namespace Application.Images;

public interface IImageService
{
    Task<Image?> UploadImagesFromBrowserAsync(Stream fileStream, string fileName, string contentType);
    Task<List<Image>> GetImagesAsync(int pageIndex, int pageSize);
    Task<Image?> GetImageAsync(Guid imageId);
    Task DeleteImageAsync(Guid imageId);
}
