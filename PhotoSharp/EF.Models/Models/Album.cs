using System.ComponentModel.DataAnnotations;

namespace EF.Models.Models;

public class Album
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public Guid? CoverImageId { get; set; }
    public List<Image> Images { get; set; } = [];

}
