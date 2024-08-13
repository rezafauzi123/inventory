using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;

public class GetRoleByIdRequest
{
    [FromRoute] public Guid RoleId { get; set; }
}