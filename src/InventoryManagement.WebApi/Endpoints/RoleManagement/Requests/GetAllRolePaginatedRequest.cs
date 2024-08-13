using InventoryManagement.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;

public class GetAllRolePaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}