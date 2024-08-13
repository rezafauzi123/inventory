using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.UserManagement.Scopes;

public class UserManagementScope : IScope
{
    public string ScopeName => nameof(UserManagementScope).ToLower();
}