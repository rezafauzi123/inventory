using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Author.Scopes;

public class AuthorScope : IScope
{
    public string ScopeName => nameof(AuthorScope).ToLower();
}