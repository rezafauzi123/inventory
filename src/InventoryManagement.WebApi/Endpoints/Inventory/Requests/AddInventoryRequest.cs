namespace InventoryManagement.WebApi.Endpoints.Inventory.Requests;

public class AddInventoryRequest
{
    public Guid BookId { get; set; }
    public int Qty { get; set; }
}