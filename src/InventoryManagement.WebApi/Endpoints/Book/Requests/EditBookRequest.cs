using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Book.Requests
{
    public class EditBookRequest
    {
        [FromRoute] public Guid Id { get; set; }
        [FromForm] public EditBookRequestPayload Payload { get; set; } = null!;
    }
}