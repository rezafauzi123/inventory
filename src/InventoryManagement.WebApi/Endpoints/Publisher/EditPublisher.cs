using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Endpoints.Publisher.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Publisher;

public class EditPublisher : BaseEndpointWithoutResponse<EditPublisherRequest>
{
    private readonly IPublisherService _PublisherService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<EditPublisher> _localizer;

    public EditPublisher(IPublisherService PublisherService,
        IDbContext dbContext,
        IStringLocalizer<EditPublisher> localizer)
    {
        _PublisherService = PublisherService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(PublisherScope))]
    [SwaggerOperation(
        Summary = "Edit Publisher",
        Description = "",
        OperationId = "Publisher.EditPublisher",
        Tags = new[] { "Publisher" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditPublisherRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditPublisherRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var publisher = await _PublisherService.GetByExpressionAsync(
            e => e.Id == request.Id,
            e => new InventoryManagement.Domain.Entities.Publisher
            {
                Id = e.Id,
                Name = e.Name,
                Phone = e.Phone,
                Email = e.Email,
                Address = e.Address,
            }, cancellationToken);

        if (publisher is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        _dbContext.AttachEntity(publisher);

        if (request.Payload.Name != publisher.Name)
            publisher.Name = request.Payload.Name!;
        
        if (request.Payload.Phone != publisher.Phone)
            publisher.Phone = request.Payload.Phone!;
        
        if (request.Payload.Email != publisher.Email)
            publisher.Email = request.Payload.Email!;
        
        if (request.Payload.Address != publisher.Address)
            publisher.Address = request.Payload.Address;
       
        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}