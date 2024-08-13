using InventoryManagement.WebApi.Scopes;

namespace InventoryManagement.WebApi.Endpoints.TransactionHistory.Scopes
{
    public class TransactionHistoryScopeReadOnly : IScope
    {
        public string ScopeName => $"{nameof(TransactionHistoryScope)}.readonly".ToLower();
    }
}