using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Endpoints.Category.Requests;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Category;

public class CreateCategory : BaseEndpointWithoutResponse<CreateCategoryRequest>
{
    private readonly IDbContext _dbContext;
    private readonly ICategoryService _CategoryService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<CreateCategory> _localizer;

    public CreateCategory(IDbContext dbContext,
        ICategoryService CategoryService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<CreateCategory> localizer)
    {
        _dbContext = dbContext;
        _CategoryService = CategoryService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
    }

    [HttpPost]
    //[Authorize]
    //[RequiredScope(typeof(CategoryScope))]
    [SwaggerOperation(
        Summary = "Create  Category",
        Description = "",
        OperationId = "Category.CreateCategory",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateCategoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var CategoryExist = await _CategoryService.IsCategoryExistAsync(request.Name!, cancellationToken);
        if (CategoryExist)
            return BadRequest(Error.Create(_localizer["name-exists"]));

        var Category = request.ToCategory(
            _rng.Generate(128, false),
            _salter);

        await _dbContext.InsertAsync(Category, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}