using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.WebApi.Endpoints.Category.Requests
{
    public class DeleteCategoryRequest
    {
        [FromRoute] public Guid Id { get; set; }
    }
}