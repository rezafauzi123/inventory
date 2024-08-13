using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Author.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Author;

public class EditAuthor : BaseEndpointWithoutResponse<EditAuthorRequest>
{
    private readonly IAuthorService _AuthorService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<EditAuthor> _localizer;
    private readonly IFileService _fileService;

    public EditAuthor(IAuthorService AuthorService,
        IDbContext dbContext,
        IStringLocalizer<EditAuthor> localizer,
        IFileService fileService)
    {
        _AuthorService = AuthorService;
        _dbContext = dbContext;
        _localizer = localizer;
        _fileService = fileService;
    }

    [HttpPut("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(AuthorScope))]
    [SwaggerOperation(
        Summary = "Edit Author",
        Description = "",
        OperationId = "author.EditAuthor",
        Tags = new[] { "Author" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditAuthorRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditAuthorRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var author = await _AuthorService.GetByExpressionAsync(
            e => e.Id == request.Id,
            e => new InventoryManagement.Domain.Entities.Author
            {
                Id = e.Id,
                Name = e.Name,
                Biography = e.Biography,
                Image = e.Image,
            }, cancellationToken);

        if (author is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        _dbContext.AttachEntity(author);

        if (request.Payload.Name != author.Name)
            author.Name = request.Payload.Name!;
        
        if (request.Payload.Biography != author.Biography)
            author.Biography = request.Payload.Biography!;

        var isfileExist = await _fileService.IsFileExistAsync(author.Image!, cancellationToken);    
        if (isfileExist)
        {
            var isSuccessDeletedFile = await _fileService.DeleteFileAsync(author.Image!, cancellationToken);
            if (!isSuccessDeletedFile)
                return BadRequest(Error.Create(_localizer["error-delete-file"]));
        }

        var fileResponse = await _fileService.UploadAsync(
            new FileRequest(request.Payload.Image.FileName, request.Payload.Image.OpenReadStream()),
            cancellationToken);

        author.Image = fileResponse.NewFileName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}