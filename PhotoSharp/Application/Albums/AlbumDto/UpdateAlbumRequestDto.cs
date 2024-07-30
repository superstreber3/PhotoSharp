using EF.Models.Models;

namespace Application.Albums.AlbumDto;

public class UpdateAlbumRequestDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid? CoverImageId { get; set; }
}
