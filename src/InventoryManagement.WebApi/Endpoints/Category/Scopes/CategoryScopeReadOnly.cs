using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Category.Scopes;

public class CategoryScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(CategoryScope)}.readonly".ToLower();
}