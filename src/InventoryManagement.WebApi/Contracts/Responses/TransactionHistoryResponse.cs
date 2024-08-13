namespace InventoryManagement.WebApi.Contracts.Responses;

public record TransactionHistoryResponse
{
    public Guid? Id { get; set; }
    public Guid? InventoryId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string? TransactionType { get; set; }    
    public int? Qty { get; set; }
}