namespace InventoryManagement.WebApi.Contracts.Responses;

public record CategoryResponse
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
}