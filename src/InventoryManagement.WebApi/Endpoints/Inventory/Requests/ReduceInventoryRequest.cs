namespace InventoryManagement.WebApi.Endpoints.Inventory.Requests;

public class ReduceInventoryRequest
{
    public Guid BookId { get; set; }
    public int Qty { get; set; }
}