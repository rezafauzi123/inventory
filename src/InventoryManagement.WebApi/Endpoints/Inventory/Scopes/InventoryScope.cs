using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Inventory.Scopes;

public class InventoryScope : IScope
{
    public string ScopeName => nameof(InventoryScope).ToLower();
}