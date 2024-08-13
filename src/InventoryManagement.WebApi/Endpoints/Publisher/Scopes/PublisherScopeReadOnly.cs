using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Scopes;

public class PublisherScopeReadOnly : IScope
{
    public string ScopeName => $"{nameof(PublisherScope)}.readonly".ToLower();
}