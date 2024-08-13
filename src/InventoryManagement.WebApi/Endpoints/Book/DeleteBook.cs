using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Book.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Book;

public class DeleteBook : BaseEndpointWithoutResponse<DeleteBookRequest>
{
    private readonly IBookService _bookService;
    private readonly IStringLocalizer<DeleteBook> _localizer;
    private readonly IFileService _fileService;

    public DeleteBook(IBookService Book, 
        IFileService fileService,
        IStringLocalizer<DeleteBook> localizer)
    {
        _bookService = Book;
        _fileService = fileService;
        _localizer = localizer;
    }

    [HttpDelete("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(BookScope))]
    [SwaggerOperation(
        Summary = "Remove data from Book  by Id",
        Description = "",
        OperationId = "Book.DeleteBook",
        Tags = new[] { "Book" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteBookRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var book = await _bookService.GetByExpressionAsync(
            e => e.Id == request.Id,
            e => new Domain.Entities.Book
            {
                Id = e.Id,
                AuthorId = e.AuthorId,
                PublisherId = e.PublisherId,
                CategoryId = e.CategoryId,
                Title = e.Title,
                Year = e.Year,
                Pages = e.Pages,
                Description = e.Description,
                PublishedDate = e.PublishedDate,
                Isbn = e.Isbn,
                Dimensions = e.Dimensions,
                Weight = e.Weight,
                Price = e.Price,
                Cover = e.Cover,
                Language = e.Language,
            }, cancellationToken);

        if (book is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        var isSuccessDeletedFile = await _fileService.DeleteFileAsync(book.Cover!, cancellationToken);
        if (!isSuccessDeletedFile)
            return BadRequest(Error.Create(_localizer["error-delete-file"]));

        await _bookService.DeleteAsync(request.Id);
        return Ok();
    }
}