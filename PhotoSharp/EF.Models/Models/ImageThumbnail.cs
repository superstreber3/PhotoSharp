using EF.Models.Models;

namespace Models.Models;

public class ImageThumbnail
{
    public Guid Id { get; set; }
    public string FilePath { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CreatedAt { get; set; }
    public Image Image { get; set; }
}
