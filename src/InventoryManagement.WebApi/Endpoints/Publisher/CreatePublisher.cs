using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.Publisher.Requests;
using InventoryManagement.WebApi.Endpoints.Publisher.Scopes;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Publisher;

public class CreatePublisher : BaseEndpointWithoutResponse<CreatePublisherRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IPublisherService _PublisherService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<CreatePublisher> _localizer;

    public CreatePublisher(IDbContext dbContext,
        IPublisherService PublisherService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<CreatePublisher> localizer)
    {
        _dbContext = dbContext;
        _PublisherService = PublisherService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
    }

    [HttpPost]
    //[Authorize]
    //[RequiredScope(typeof(PublisherScope))]
    [SwaggerOperation(
        Summary = "Create Publisher",
        Description = "",
        OperationId = "Publisher.CreatePublisher",
        Tags = new[] { "Publisher" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreatePublisherRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreatePublisherRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var publisherExist = await _PublisherService.IsPublisherExistAsync(request.Name!, cancellationToken);
        if (publisherExist)
            return BadRequest(Error.Create(_localizer["name-exists"]));

        var publisher = request.ToPublisher(
            _rng.Generate(128, false),
            _salter);

        await _dbContext.InsertAsync(publisher, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}