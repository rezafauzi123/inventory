using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Book.Scopes;

public class BookScope : IScope
{
    public string ScopeName => nameof(BookScope).ToLower();
}