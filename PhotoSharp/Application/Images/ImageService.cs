using Models;
using Models.Models;
using System.Net.Mime;

namespace Application.Images;

public class ImageService(AppDbContext appDbContext) : IImageService
{
    public async Task<Image> UploadImagesFromBrowserAsync(Stream fileStream, string fileName, string contentType)
    {
        //write the file to the file system (storage folder on C: drive)
        var path = $"C:\\storage\\{fileName}{DateTime.Now:yy-MM-ddTHH-mm-ss}.png";
        await using (var fs = new FileStream(path, FileMode.Create))
        {
            await fileStream.CopyToAsync(fs);
        }

        int width;
        int height;

        using (var img = await SixLabors.ImageSharp.Image.LoadAsync(path))
        {
            width = img.Width;
            height = img.Height;
        }

        //save image to the database
        var image = new Image
        {
            FileName = fileName,
            FilePath = path,
            Width = width,
            Height = height,
            ContentType = contentType,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };
        
        await appDbContext.Images.AddAsync(image);
        await appDbContext.SaveChangesAsync();
        return image;
    }
}
