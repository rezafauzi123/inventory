using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.Author.Requests;
using InventoryManagement.WebApi.Endpoints.Author.Scopes;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Author;

public class CreateAuthor : BaseEndpointWithoutResponse<CreateAuthorRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IAuthorService _AuthorService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<CreateAuthor> _localizer;
    private readonly IFileService _fileService;

    public CreateAuthor(IDbContext dbContext,
        IAuthorService AuthorService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<CreateAuthor> localizer,
        IFileService fileService)
    {
        _dbContext = dbContext;
        _AuthorService = AuthorService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
        _fileService = fileService;
    }

    [HttpPost]
    //[Authorize]
    //[RequiredScope(typeof(AuthorScope))]
    [SwaggerOperation(
        Summary = "Create Author",
        Description = "",
        OperationId = "Author.CreateAuthor",
        Tags = new[] { "Author" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateAuthorRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateAuthorRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var authorExist = await _AuthorService.IsAuthorExistAsync(request.Name!, cancellationToken);
        if (authorExist)
            return BadRequest(Error.Create(_localizer["name-exists"]));
        
        var fileResponse = await _fileService.UploadAsync(
            new FileRequest(request.Image.FileName, request.Image.OpenReadStream()),
            cancellationToken);

        var author = request.ToAuthor(
            _rng.Generate(128, false),
            _salter);

        author.Image = fileResponse.NewFileName;

        await _dbContext.InsertAsync(author, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}