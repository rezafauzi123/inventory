namespace InventoryManagement.WebApi.Contracts.Responses;

public record InventoryResponse
{
    public Guid? Id { get; set; }
    public string? BookCode { get; set; }
    public string? BookTitle { get; set; }
    public int? Stock { get; set; }
    public string? Author { get; set; }
    public string? Category { get; set; }
    public string? Publisher { get; set; }
    public int? Year { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? Isbn { get; set; }
    public int? Price { get; set; }
    public string? Language { get; set; }
}