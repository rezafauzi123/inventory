using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.UserManagement.Requests;

public class UpdateUserRequest
{
    public UpdateUserRequest()
    {
        UpdateUserRequestPayload = new UpdateUserRequestPayload();
    }

    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
    [FromBody] public UpdateUserRequestPayload UpdateUserRequestPayload { get; set; }
}

public class UpdateUserRequestPayload
{
    public string? Fullname { get; set; }
}