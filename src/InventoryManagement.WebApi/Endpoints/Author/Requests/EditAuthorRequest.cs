using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Author.Requests
{
    public class EditAuthorRequest
    {
        [FromRoute] public Guid Id { get; set; }
        [FromForm] public EditAuthorRequestPayload Payload { get; set; } = null!;
    }
}