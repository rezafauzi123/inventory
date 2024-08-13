using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Category.Requests;

public class GetAllCategoryRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}