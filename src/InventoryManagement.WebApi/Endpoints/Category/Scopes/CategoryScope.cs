using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Category.Scopes;

public class CategoryScope : IScope
{
    public string ScopeName => nameof(CategoryScope).ToLower();
}