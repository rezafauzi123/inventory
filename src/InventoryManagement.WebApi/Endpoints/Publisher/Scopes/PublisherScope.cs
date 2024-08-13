using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Scopes;

public class PublisherScope : IScope
{
    public string ScopeName => nameof(PublisherScope).ToLower();
}