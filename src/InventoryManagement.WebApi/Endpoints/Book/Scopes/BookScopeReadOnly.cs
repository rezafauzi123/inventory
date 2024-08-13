using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Book.Scopes;

public class BookScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(BookScope)}.readonly".ToLower();
}