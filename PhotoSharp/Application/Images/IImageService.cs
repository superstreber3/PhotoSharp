using Models.Models;

namespace Application.Images;

public interface IImageService
{
    Task<Image> UploadImagesFromBrowserAsync(Stream fileStream, string fileName, string contentType);
}
