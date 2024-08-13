using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Scopes;

public class RoleManagementScope : IScope
{
    public string ScopeName => nameof(RoleManagementScope).ToLower();
}