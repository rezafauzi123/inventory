using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Category.Requests
{
    public class EditCategoryRequest
    {
        [FromRoute] public Guid Id { get; set; }
        [FromBody] public EditCategoryRequestPayload Payload { get; set; } = null!;
    }
}