namespace InventoryManagement.WebApi.Contracts.Responses;

public record InventoryResponse
{
    public Guid? Id { get; set; }
    public int? Stock { get; set; }
    public BookResponse? Book { get; set; }   
}