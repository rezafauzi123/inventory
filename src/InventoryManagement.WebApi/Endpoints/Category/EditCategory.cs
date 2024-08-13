using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Endpoints.Category.Requests;
using InventoryManagement.WebApi.Endpoints.Inventory.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Category;

public class EditCategory : BaseEndpointWithoutResponse<EditCategoryRequest>
{
    private readonly ICategoryService _CategoryService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<EditCategory> _localizer;

    public EditCategory(ICategoryService CategoryService,
        IDbContext dbContext,
        IStringLocalizer<EditCategory> localizer)
    {
        _CategoryService = CategoryService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(CategoryScope))]
    [SwaggerOperation(
        Summary = "Edit  Category",
        Description = "",
        OperationId = "Category.EditCategory",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditCategoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditCategoryRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var Category = await _CategoryService.GetByExpressionAsync(
            e => e.Id == request.Id,
            e => new Domain.Entities.Category
            {
                Id = e.Id,
                Name = e.Name
            }, cancellationToken);

        if (Category is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        _dbContext.AttachEntity(Category);

        if (request.Payload.Name != Category.Name)
            Category.Name = request.Payload.Name!;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}