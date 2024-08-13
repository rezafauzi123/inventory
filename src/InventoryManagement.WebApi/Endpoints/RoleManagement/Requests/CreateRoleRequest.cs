namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;

public class CreateRoleRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<string> Scopes { get; set; } = new();
}