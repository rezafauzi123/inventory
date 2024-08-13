using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.UserManagement.Requests;

public class SetUserInActiveRequest
{
    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
}