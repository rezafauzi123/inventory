namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests;

public class CreatePublisherRequest
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
}