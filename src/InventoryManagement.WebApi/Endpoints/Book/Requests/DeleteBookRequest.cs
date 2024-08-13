using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Book.Requests
{
    public class DeleteBookRequest
    {
        [FromRoute] public Guid Id { get; set; }
    }
}