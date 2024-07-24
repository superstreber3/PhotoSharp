namespace Models.Models;

public class Image
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string ContentType { get; set; }
    public DateTime CreatedAt { get; set; }
}
