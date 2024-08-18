using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Book.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Book;

public class GetAllBook : BaseEndpoint<GetAllBookRequest, List<BookResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllBook(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(BookScope), typeof(BookScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get All Book",
        Description = "",
        OperationId = "Book.GetAllBook",
        Tags = new[] { "Book" })
    ]
    [ProducesResponseType(typeof(List<BookResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<BookResponse>>> HandleAsync([FromQuery] GetAllBookRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Domain.Entities.Book>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Title, $"%{request.Search}%"));

        var data = await queryable
            .Select(e => new BookResponse
            {
                Id = e.Id,
                AuthorId = e.AuthorId,
                PublisherId = e.PublisherId,
                CategoryId = e.CategoryId,
                Code = e.Code,
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
                Author = new AuthorResponse
                {
                    Id = e.Author!.Id,
                    Name = e.Author!.Name,
                    Biography = e.Author.Biography,
                    Image = e.Author.Image
                },
                Category = new CategoryResponse
                {
                    Id = e.Category!.Id
                },
                Publisher = new PublisherResponse
                {
                    Id = e.Publisher!.Id,
                }
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}