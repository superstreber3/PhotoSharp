using Application.Albums.AlbumDto;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Application.Albums;

public class AlbumService(AppDbContext appDbContext) : IAlbumService
{

    public async Task<Album> CreateAlbumAsync(CreateAlbumRequestDto createAlbumRequest)
    {
        if (createAlbumRequest.Name == null || createAlbumRequest.Description == null)
        {
            throw new Exception("Name and Description are required");
        }
        var coverImage = await appDbContext.Images.FindAsync(createAlbumRequest.CoverImageId);
        var album = new Album
        {
            Name = createAlbumRequest.Name,
            Description = createAlbumRequest.Description,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            LastUpdatedAt = DateTime.Now.ToUniversalTime(),
            CoverImageId = coverImage?.Id
        };
        await appDbContext.Albums.AddAsync(album);
        await appDbContext.SaveChangesAsync();
        return album;
    }
    public async Task<Album?> GetAlbumAsync(Guid id)
    {
        return await appDbContext.Albums
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public async Task<List<Album>> GetAlbumsAsync()
    {
        return await appDbContext.Albums.ToListAsync();
    }
    public async Task<Album> UpdateAlbumAsync(UpdateAlbumRequestDto updateAlbumRequest)
    {
        var album = await appDbContext.Albums.FirstOrDefaultAsync(a => a.Id == updateAlbumRequest.Id);
        if (album == null)
        {
            throw new Exception("Album not found");
        }
        var coverImage = await appDbContext.Images.FindAsync(updateAlbumRequest.CoverImageId);
        album.Name = updateAlbumRequest.Name;
        album.Description = updateAlbumRequest.Description;
        if (coverImage != null)
            album.CoverImageId = updateAlbumRequest.CoverImageId;
        album.LastUpdatedAt = DateTime.Now.ToUniversalTime();
        await appDbContext.SaveChangesAsync();
        return album;
    }
    public async Task DeleteAlbumAsync(Guid id)
    {
        var album = await appDbContext.Albums.FindAsync(id);
        if (album == null)
        {
            throw new Exception("Album not found");
        }
        appDbContext.Albums.Remove(album);
        await appDbContext.SaveChangesAsync();
    }
    public async Task AddImageToAlbumAsync(Guid albumId, Guid imageId)
    {
        var album = await appDbContext.Albums
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == albumId);
        if (album == null) return;

        var image = await appDbContext.Images.FindAsync(imageId);
        if (image == null) return;

        //check if the image is already in the album
        if (album.Images.Any(i => i.Id == imageId)) return;

        album.Images.Add(image);
        await appDbContext.SaveChangesAsync();
    }

    public async Task AddImagesToAlbumAsync(Guid albumId, List<Guid> imageIds)
    {
        var album = await appDbContext.Albums
            .Include(a => a.Images)
            .FirstOrDefaultAsync(a => a.Id == albumId);
        if (album == null) return;

        var images = await appDbContext.Images.Where(i => imageIds.Contains(i.Id)).ToListAsync();
        if (images.Count == 0) return;

        album.Images.AddRange(images);
        await appDbContext.SaveChangesAsync();
    }
    public async Task RemoveImageFromAlbumAsync(Guid albumId, Guid imageId)
    {
        var album = await appDbContext.Albums.FindAsync(albumId);
        if (album == null) return;

        var image = await appDbContext.Images.FindAsync(imageId);
        if (image == null) return;

        album.Images.Remove(image);
        await appDbContext.SaveChangesAsync();
    }
}
