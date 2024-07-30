using EF.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Albums.AlbumDto;

public class CreateAlbumRequestDto
{
    [Required(ErrorMessage = "Album Name is required")]
    [StringLength(100, ErrorMessage = "Album Name can't be longer than 100 characters")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters")]
    public string? Description { get; set; }
    public Guid? CoverImageId { get; set; }
}
