using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Author.Scopes;

public class AuthorScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(AuthorScope)}.readonly".ToLower();
}