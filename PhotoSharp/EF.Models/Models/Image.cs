using Models.Models;
using System.ComponentModel.DataAnnotations;

namespace EF.Models.Models;

public class Image
{
    public Guid Id { get; set; }
    [MaxLength(100)]
    public required string FileName { get; set; }
    [MaxLength(100)]
    public required string FilePath { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [MaxLength(100)]
    public required string ContentType { get; set; }
    public DateTime CreatedAt { get; set; }
    public required List<ImageThumbnail> Thumbnail { get; set; }
}
