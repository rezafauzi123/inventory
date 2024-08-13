namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;

public class EditRoleRequestPayload
{
    public string? Description { get; set; }
    public List<string> Scopes { get; set; } = new();
}