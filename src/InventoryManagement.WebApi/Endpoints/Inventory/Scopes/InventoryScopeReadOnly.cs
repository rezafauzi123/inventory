using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Inventory.Scopes
{
    public class InventoryScopeReadOnly : IScope
    {
        public string ScopeName => $"{nameof(InventoryScope)}.readonly".ToLower();
    }
}