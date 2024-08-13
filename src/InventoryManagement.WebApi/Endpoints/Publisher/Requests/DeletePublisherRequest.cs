using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests
{
    public class DeletePublisherRequest
    {
        [FromRoute] public Guid Id { get; set; }
    }
}