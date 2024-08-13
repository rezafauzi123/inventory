using InventoryManagement.Core.Abstractions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.UserManagement.Requests;
using InventoryManagement.WebApi.Endpoints.UserManagement.Scopes;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.UserManagement;

public class CreateUser : BaseEndpointWithoutResponse<CreateUserRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<CreateUser> _localizer;

    public CreateUser(IDbContext dbContext,
        IUserService userService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<CreateUser> localizer)
    {
        _dbContext = dbContext;
        _userService = userService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
    }

    [HttpPost("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Create user API",
        Description = "",
        OperationId = "UserManagement.CreateUser",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateUserRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var userExist = await _userService.IsUserExistAsync(request.Username!, cancellationToken);
        if (userExist)
            return BadRequest(Error.Create(_localizer["username-exists"]));

        var user = request.ToUser(
            _rng.Generate(128, false),
            _salter);

        await _dbContext.InsertAsync(user, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}