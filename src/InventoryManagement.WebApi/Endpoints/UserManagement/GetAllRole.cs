using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.UserManagement;

public class GetAllRole : BaseEndpoint<GetAllRoleRequest, List<RoleResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllRole(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users/roles")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles",
        Description = "",
        OperationId = "UserManagement.GetAllRole",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(List<RoleResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<RoleResponse>>> HandleAsync([FromQuery] GetAllRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Role>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        var data = await queryable
            .Select(e => new RoleResponse
            {
                RoleId = e.RoleId,
                Name = e.Name,
                Description = e.Description
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}