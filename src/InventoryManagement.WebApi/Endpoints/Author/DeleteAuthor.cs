using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Author.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Author;

public class DeleteAuthor : BaseEndpointWithoutResponse<DeleteAuthorRequest>
{
    private readonly IAuthorService _authorService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<DeleteAuthor> _localizer;
    private readonly IFileService _fileService;

    public DeleteAuthor(IAuthorService authorService, 
        IFileService fileService,
        IStringLocalizer<DeleteAuthor> localizer)
    {
        _authorService = authorService;
        _fileService = fileService;
        _localizer = localizer;
    }

    [HttpDelete("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(AuthorScope))]
    [SwaggerOperation(
        Summary = "Remove data from Author by Id",
        Description = "",
        OperationId = "Author.DeleteAuthor",
        Tags = new[] { "Author" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteAuthorRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var author = await _authorService.GetByExpressionAsync(
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

        var isSuccessDeletedFile = await _fileService.DeleteFileAsync(author.Image!, cancellationToken);
        if (!isSuccessDeletedFile)
            return BadRequest(Error.Create(_localizer["error-delete-file"]));

        await _authorService.DeleteAsync(request.Id);
        return Ok();
    }
}