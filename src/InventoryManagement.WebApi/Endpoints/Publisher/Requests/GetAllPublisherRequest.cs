using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests;

public class GetAllPublisherRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}