using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.TransactionHistory.Scopes;

public class TransactionHistoryScope : IScope
{
    public string ScopeName => nameof(TransactionHistoryScope).ToLower();
}