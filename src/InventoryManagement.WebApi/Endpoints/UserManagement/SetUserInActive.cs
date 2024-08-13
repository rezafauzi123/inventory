using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Enums;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.UserManagement.Requests;
using InventoryManagement.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.UserManagement;

public class SetUserInActive : BaseEndpointWithoutResponse<SetUserInActiveRequest>
{
    private readonly IUserService _userService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<SetUserInActive> _localizer;

    public SetUserInActive(IUserService userService,
        IDbContext dbContext,
        IStringLocalizer<SetUserInActive> localizer)
    {
        _userService = userService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPatch("users/{userId}/inactive")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Set user inactive",
        Description = "",
        OperationId = "UserManagement.GetById",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] SetUserInActiveRequest request,
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetByExpressionAsync(
            e => e.UserId == request.UserId,
            e => new User
            {
                UserId = e.UserId,
                StatusRecord = e.StatusRecord
            }, cancellationToken);

        if (user is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        if (user.StatusRecord == StatusRecord.InActive)
            return BadRequest(Error.Create(_localizer["user-inactive"]));

        _dbContext.AttachEntity(user);

        user.StatusRecord = StatusRecord.InActive;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}