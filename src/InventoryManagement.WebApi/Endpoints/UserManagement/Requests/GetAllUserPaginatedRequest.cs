using InventoryManagement.Shared.Abstractions.Queries;

namespace InventoryManagement.WebApi.Endpoints.UserManagement.Requests;

public class GetAllUserPaginatedRequest : BasePaginationCalculation
{
    public string? Username { get; set; }
    public string? Fullname { get; set; }
}