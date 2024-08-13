using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;

public class GetAllRoleRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}