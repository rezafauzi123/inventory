using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Clock;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Identity.Helpers;
using InventoryManagement.WebApi.Endpoints.Identity.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Identity;

public class SignIn : BaseEndpoint<SignInRequest, LoginResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IClock _clock;
    private readonly IAuthManager _authManager;
    private readonly IStringLocalizer<SignIn> _localizer;
    private readonly IAES _iaes;

    public SignIn(IDbContext dbContext,
        IUserService userService,
        IClock clock,
        IAuthManager authManager,
        IStringLocalizer<SignIn> localizer,
        IAES iaes
        )
    {
        _dbContext = dbContext;
        _userService = userService;
        _clock = clock;
        _authManager = authManager;
        _localizer = localizer;
        _iaes = iaes;
    }

    [HttpPost("sign-in")]
    [SwaggerOperation(
        Summary = "Sign in",
        Description = "",
        OperationId = "Identity.SignIn",
        Tags = new[] { "Identity" })
    ]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(SignInRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new SignInRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var user = await _userService.GetByUsernameAsync(request.Username!, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create(_localizer["invalid-username-password"]));

        if (!_userService.VerifyPassword(user.Password!, user.Salt!, request.Password!))
            return BadRequest(Error.Create(_localizer["invalid-username-password"]));

        _dbContext.AttachEntity(user);

        var newUserToken = new UserToken
        {
            UserId = user.UserId,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiryAt = _clock.CurrentDate().Add(_authManager.Options.RefreshTokenExpiry),
        };
        user.UserTokens.Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var claims = ClaimsGenerator.Generate(user, _iaes);

        var token = _authManager.CreateToken(
            newUserToken.UserTokenId.ToString("N"),
            _authManager.Options.Audience,
            claims);

        var dto = new LoginResponse(user)
        {
            AccessToken = token.AccessToken,
            Expiry = token.Expiry,
            RefreshToken = newUserToken.RefreshToken
        };

        return Ok(dto);
    }
}