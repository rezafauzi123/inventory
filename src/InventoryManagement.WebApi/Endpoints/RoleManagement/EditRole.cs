using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Scopes;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement;

public class EditRole : BaseEndpointWithoutResponse<EditRoleRequest>
{
    private readonly IRoleService _roleService;
    private readonly IDbContext _dbContext;
    private readonly IStringLocalizer<EditRole> _localizer;

    public EditRole(IRoleService roleService,
        IDbContext dbContext,
        IStringLocalizer<EditRole> localizer)
    {
        _roleService = roleService;
        _dbContext = dbContext;
        _localizer = localizer;
    }

    [HttpPut("roles/{RoleId}")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope))]
    [SwaggerOperation(
        Summary = "Edit role API",
        Description = "",
        OperationId = "RoleManagement.EditRole",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditRoleRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

        var role = await _roleService.GetByExpressionAsync(
            e => e.RoleId == request.RoleId,
            e => new Role
            {
                RoleId = e.RoleId,
                Description = e.Description,
                RoleScopes = e.RoleScopes.Select(f => new RoleScope
                {
                    RoleScopeId = f.RoleScopeId,
                    Name = f.Name
                }).ToList()
            }, cancellationToken);

        if (role is null)
            return BadRequest(Error.Create(_localizer["data-not-found"]));

        _dbContext.AttachEntity(role);

        if (request.Payload.Description != role.Description)
            role.Description = request.Payload.Description;

        foreach (var item in role.RoleScopes)
            if (request.Payload.Scopes.All(e => e != item.Name))
                item.SetToDeleted();

        foreach (var item in request.Payload.Scopes)
            if (role.RoleScopes.All(e => e.Name != item))
                role.RoleScopes.Add(new RoleScope
                {
                    RoleId = role.RoleId,
                    Name = item
                });

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}