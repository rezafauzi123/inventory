using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Author.Requests;

public class GetAllAuthorRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}