using InventoryManagement.Core.Abstractions;
using InventoryManagement.WebApi.Contracts.Requests;
using InventoryManagement.WebApi.Endpoints.Category.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Category;

public class DeleteCategory : BaseEndpointWithoutResponse<DeleteCategoryRequest>
{
    private readonly ICategoryService _Category;

    public DeleteCategory(ICategoryService Category)
    {
        _Category = Category;
    }

    [HttpDelete("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(CategoryScope))]
    [SwaggerOperation(
        Summary = "Remove data from  Category by Id",
        Description = "",
        OperationId = "Category.DeleteCategory",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteCategoryRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await _Category.DeleteAsync(request.Id);
        return Ok();
    }
}