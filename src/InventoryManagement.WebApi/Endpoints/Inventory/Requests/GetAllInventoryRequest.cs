using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Inventory.Requests;

public class GetAllInventoryRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}