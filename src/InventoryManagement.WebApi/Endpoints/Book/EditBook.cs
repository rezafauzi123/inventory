using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Book.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Book;

public class EditBook : BaseEndpointWithoutResponse<EditBookRequest>
{
    private readonly IBookService _bookService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<EditBook> _localizer;
    private readonly IFileService _fileService;

    public EditBook(IBookService bookService,
        IDbContext dbContext,
        IStringLocalizer<EditBook> localizer,
        IFileService fileService)
    {
        _bookService = bookService;
        _dbContext = dbContext;
        _localizer = localizer;
        _fileService = fileService;
    }

    [HttpPut("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(BookScope))]
    [SwaggerOperation(
        Summary = "Edit Book ",
        Description = "",
        OperationId = "Book.EditBook",
        Tags = new[] { "Book" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditBookRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditBookRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var isbnValidator = new Common.IsbnValidator();
        var isbnValidatorResult = isbnValidator.IsValidISBNCode(request.Payload.Isbn!);
        if (!isbnValidatorResult)
            return BadRequest(Error.Create(_localizer["invalid-isbn"]));

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
                Pages= e.Pages,
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

        _dbContext.AttachEntity(book);

        if (request.Payload.Title != book.Title)
            book.Title = request.Payload.Title!;

        var isfileExist = await _fileService.IsFileExistAsync(book.Cover!, cancellationToken);
        if (isfileExist)
        {
            var isSuccessDeletedFile = await _fileService.DeleteFileAsync(book.Cover!, cancellationToken);
            if (!isSuccessDeletedFile)
                return BadRequest(Error.Create(_localizer["error-delete-file"]));
        }

        var fileResponse = await _fileService.UploadAsync(
            new FileRequest(request.Payload.Cover.FileName, request.Payload.Cover.OpenReadStream()),
            cancellationToken);

        book.Cover = fileResponse.NewFileName;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}