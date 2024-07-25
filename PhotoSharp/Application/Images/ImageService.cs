﻿using Application.Settings;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Models.Models;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Application.Images;

public class ImageService(AppDbContext appDbContext, IOptions<SettingsOptions> settings) : IImageService
{
    public async Task<Image?> UploadImagesFromBrowserAsync(Stream fileStream, string fileName, string contentType)
    {
        //write the file to the file system (storage folder on C: drive)
        var path = $"{settings.Value.RootFolder}\\{fileName}{DateTime.Now:yy-MM-ddTHH-mm-ss}.{GetContentType(contentType)}";
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
            CreatedAt = DateTime.Now.ToUniversalTime(),
            Thumbnail = [await GenerateThumbnailForImage(fileName, contentType, path)]
        };

        await appDbContext.Images.AddAsync(image);
        await appDbContext.SaveChangesAsync();
        return image;
    }

    public async Task<List<Image>> GetImagesAsync(int pageIndex, int pageSize)
    {
        return await appDbContext.Images
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Image?> GetImageAsync(Guid imageId)
    {
        return await appDbContext.Images
            .Include(x => x.Thumbnail)
            .FirstOrDefaultAsync(x => x.Id == imageId);
    }

    private async Task<ImageThumbnail> GenerateThumbnailForImage(string filename, string contentType, string filePath)
    {
        var thumbnailPath = $"{settings.Value.RootFolder}\\thumbnails\\{filename}{DateTime.Now:yy-MM-ddTHH-mm-ss}.{GetContentType(contentType)}";
        //copy image to the thumbnail folder
        //resize the image
        using var img = await SixLabors.ImageSharp.Image.LoadAsync(filePath);
        //rezise the image to a width of 100px and keep the aspect ratio if the width is already less than 100px or equal to 100px then the image will not be resized
        var width = img.Width > 250 ? 250 : img.Width;
        var height = img.Height * width / img.Width;
        img.Mutate(x => x.Resize(width, height));
        await using var fileStream = new FileStream(thumbnailPath, FileMode.Create);
        //same encoding as the original image
        await img.SaveAsync(fileStream, GetEncoder(contentType));
        var thumbnail = new ImageThumbnail
        {
            FilePath = thumbnailPath,
            Width = width,
            Height = height,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };

        return thumbnail;

    }

    private IImageEncoder GetEncoder(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => new JpegEncoder(),
            "image/png" => new PngEncoder(),
            "image/gif" => new GifEncoder(),
            _ => throw new Exception("Unsupported content type")
        };
    }

    private string GetContentType(string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            _ => throw new Exception("Unsupported content type")
        };
    }
}