using InventoryManagement.Core.Abstractions;
using InventoryManagement.WebApi.Contracts.Requests;
using InventoryManagement.WebApi.Endpoints.Publisher.Requests;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Publisher;

public class DeletePublisher : BaseEndpointWithoutResponse<DeletePublisherRequest>
{
    private readonly IPublisherService _Publisher;

    public DeletePublisher(IPublisherService Publisher)
    {
        _Publisher = Publisher;
    }

    [HttpDelete("{Id}")]
    //[Authorize]
    //[RequiredScope(typeof(PublisherScope))]
    [SwaggerOperation(
        Summary = "Remove data from Publisher by Id",
        Description = "",
        OperationId = "Publisher.DeletePublisher",
        Tags = new[] { "Publisher" })
    ]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeletePublisherRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        await _Publisher.DeleteAsync(request.Id);
        return Ok();
    }
}