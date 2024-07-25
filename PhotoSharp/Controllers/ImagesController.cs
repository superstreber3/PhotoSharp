using Application.Images;
using Microsoft.AspNetCore.Mvc;

namespace PhotoSharp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController(IImageService imageService) : ControllerBase
{
    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetImageFile(Guid fileId)
    {
        var image = await imageService.GetImageAsync(fileId);

        if (!System.IO.File.Exists(image?.FilePath))
        {
            return NotFound();
        }

        var memory = new MemoryStream();
        await using (var stream = new FileStream(image.FilePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        //get content type of the file
        return File(memory, image.ContentType);
    }

    [HttpGet("{fileId}/thumbnail")]
    public async Task<IActionResult> GetImageThumbnailFile(Guid fileId)
    {
        var image = await imageService.GetImageAsync(fileId);
        var thumbnail = image?.Thumbnail;

        if (thumbnail == null || !System.IO.File.Exists(thumbnail.FirstOrDefault()?.FilePath))
        {
            return NotFound();
        }

        var memory = new MemoryStream();
        await using (var stream = new FileStream(thumbnail.First().FilePath, FileMode.Open))
        {
            await stream.CopyToAsync(memory);
        }
        memory.Position = 0;

        //get content type of the file
        return File(memory, image!.ContentType);
    }
}
