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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Identity;

public class RefreshToken : BaseEndpoint<RefreshTokenRequest, LoginResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IUserService _userService;
    private readonly IAuthManager _authManager;
    private readonly IStringLocalizer<RefreshToken> _localizer;
    private readonly IAES _iaes;

    public RefreshToken(IDbContext dbContext,
        IClock clock,
        IUserService userService,
        IAuthManager authManager,
        IStringLocalizer<RefreshToken> localizer,
        IAES iaes)
    {
        _dbContext = dbContext;
        _clock = clock;
        _userService = userService;
        _authManager = authManager;
        _localizer = localizer;
        _iaes = iaes;
    }

    [HttpPost("refresh")]
    [SwaggerOperation(
        Summary = "Refresh token",
        Description = "",
        OperationId = "Identity.Refresh",
        Tags = new[] { "Identity" })
    ]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new RefreshTokenRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var userToken = await _dbContext.Set<UserToken>()
            .Where(e => e.RefreshToken == request.RefreshToken)
            .Select(e => new UserToken
            {
                UserTokenId = e.UserTokenId,
                RefreshToken = e.RefreshToken,
                IsUsed = e.IsUsed,
                ExpiryAt = e.ExpiryAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userToken is null || userToken.IsUsed || userToken.ExpiryAt < _clock.CurrentDate())
            return BadRequest(Error.Create(_localizer["invalid-request"]));

        _dbContext.AttachEntity(userToken);

        userToken.IsUsed = true;

        var newUserToken = new UserToken
        {
            UserId = userToken.UserId,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiryAt = _clock.CurrentDate().Add(_authManager.Options.RefreshTokenExpiry),
        };

        _dbContext.Set<UserToken>().Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _userService.GetByIdAsync(userToken.UserId, cancellationToken);

        var claims = ClaimsGenerator.Generate(user!, _iaes);

        var token = _authManager.CreateToken(
            newUserToken.UserTokenId.ToString("N"),
            _authManager.Options.Audience,
            claims);

        var dto = new LoginResponse(user!)
        {
            AccessToken = token.AccessToken,
            Expiry = token.Expiry,
            RefreshToken = newUserToken.RefreshToken
        };

        return Ok(dto);
    }
}