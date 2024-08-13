using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests
{
    public class EditPublisherRequest
    {
        [FromRoute] public Guid Id { get; set; }
        [FromBody] public EditPublisherRequestPayload Payload { get; set; } = null!;
    }
}