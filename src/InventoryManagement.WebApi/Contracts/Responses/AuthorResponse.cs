namespace InventoryManagement.WebApi.Contracts.Responses;

public record AuthorResponse
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Biography { get; set; }
    public string? Image { get; set; }
}