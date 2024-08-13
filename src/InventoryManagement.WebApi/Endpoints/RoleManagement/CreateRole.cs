using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Scopes;
using InventoryManagement.WebApi.Mapping;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement;

public class CreateRole : BaseEndpointWithoutResponse<CreateRoleRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<CreateRole> _localizer;

    public CreateRole(IDbContext dbContext,
        IStringLocalizer<CreateRole> localizer)
    {
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPost("roles")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope))]
    [SwaggerOperation(
        Summary = "Create role API",
        Description = "",
        OperationId = "RoleManagement.CreateRole",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateRoleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var roleIsExists = await _dbContext.Set<Role>().Where(e => e.Name == request.Name)
            .FirstOrDefaultAsync(cancellationToken);
        if (roleIsExists != null)
            return BadRequest(Error.Create(string.Format(_localizer["role-name-exists"], request.Name)));

        var role = request.ToRole();

        await _dbContext.InsertAsync(role, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}