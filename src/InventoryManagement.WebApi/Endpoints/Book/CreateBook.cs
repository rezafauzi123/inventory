using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Book.Requests;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Book;

public class CreateBook : BaseEndpointWithoutResponse<CreateBookRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IBookService _bookService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<CreateBook> _localizer;
    private readonly IFileService _fileService;

    public CreateBook(IDbContext dbContext,
        IBookService BookService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<CreateBook> localizer,
        IFileService fileService)
    {
        _dbContext = dbContext;
        _bookService = BookService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
        _fileService = fileService;
    }

    [HttpPost]
    //[Authorize]
    //[RequiredScope(typeof(BookScope))]
    [SwaggerOperation(
        Summary = "Create Book ",
        Description = "",
        OperationId = "Book.CreateBook",
        Tags = new[] { "Book" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateBookRequest request,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var validator = new CreateBookRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

            var isbnValidator = new Common.IsbnValidator();
            var isbnValidatorResult = isbnValidator.IsValidISBNCode(request.Isbn!);
            if (!isbnValidatorResult)
                return BadRequest(Error.Create(_localizer["invalid-isbn"]));

            var bookExist = await _bookService.IsBookExistAsync(request.Title!, cancellationToken);
            if (bookExist)
                return BadRequest(Error.Create(_localizer["name-exists"]));

            var fileResponse = await _fileService.UploadAsync(
                new FileRequest(request.Cover.FileName, request.Cover.OpenReadStream()),
                cancellationToken);

            var book = request.ToBook(
                _rng.Generate(128, false),
            _salter);

            book.Cover = fileResponse.NewFileName;

            await _dbContext.InsertAsync(book, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new(ex.InnerException!.Message);
        }
    }
}