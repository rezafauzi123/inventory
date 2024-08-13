using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Author.Requests
{
    public class DeleteAuthorRequest
    {
        [FromRoute] public Guid Id { get; set; }
    }
}