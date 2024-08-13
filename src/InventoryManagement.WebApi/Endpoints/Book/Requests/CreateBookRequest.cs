namespace InventoryManagement.WebApi.Endpoints.Book.Requests;

public class CreateBookRequest
{
    public Guid? AuthorId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? PublisherId { get; set; }
    public string? Title { get; set; }
    public int? Year { get; set; }
    public int? Pages { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? Isbn { get; set; }
    public string? Dimensions { get; set; }
    public int? Weight { get; set; }
    public int? Price { get; set; }
    public IFormFile Cover { get; set; } = null!;
    public string? Language { get; set; }
}