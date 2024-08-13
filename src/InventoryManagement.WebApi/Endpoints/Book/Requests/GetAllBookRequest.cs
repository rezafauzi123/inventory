using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Book.Requests;

public class GetAllBookRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}