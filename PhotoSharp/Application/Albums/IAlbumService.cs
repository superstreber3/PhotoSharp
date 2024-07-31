using Application.Albums.AlbumDto;
using EF.Models.Models;

namespace Application.Albums;

public interface IAlbumService
{
    public Task<Album> CreateAlbumAsync(CreateAlbumRequestDto createAlbumRequest);
    public Task<Album?> GetAlbumAsync(Guid id);
    public Task<List<Album>> GetAlbumsAsync();
    public Task<Album> UpdateAlbumAsync(UpdateAlbumRequestDto updateAlbumRequest);
    public Task DeleteAlbumAsync(Guid id);
    public Task AddImageToAlbumAsync(Guid albumId, Guid imageId);
    public Task AddImagesToAlbumAsync(Guid albumId, List<Guid> imageId);
    public Task RemoveImageFromAlbumAsync(Guid albumId, Guid imageId);
}
