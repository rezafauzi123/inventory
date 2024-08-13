using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.TransactionHistory.Requests;

public class GetAllTransactionHistoryRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}