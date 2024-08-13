using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Contexts;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Identity;

public class GetMe : BaseEndpoint<UserResponse>
{
    private readonly IContext _context;
    private readonly IUserService _userService;
    private readonly IStringLocalizer<GetMe> _localizer;

    public GetMe(IContext context,
        IUserService userService,
        IStringLocalizer<GetMe> localizer)
    {
        _context = context;
        _userService = userService;
        _localizer = localizer;
    }

    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Get me",
        Description = "Get profile by user id of its user",
        OperationId = "Identity.GetMe",
        Tags = new[] { "Identity" })
    ]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserResponse>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetByIdAsync(_context.Identity.Id, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        var dto = user.ToUserResponse();

        if (user.UserRoles.All(e => e.RoleId != RoleExtensions.SuperAdministratorId))
            return Ok(dto);

        dto.Scopes.Clear();
        dto.Scopes.AddRange(ScopeManager.Instance.GetAllScopes());

        return Ok(dto);
    }
}