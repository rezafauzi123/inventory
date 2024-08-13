using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Queries;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq.Dynamic.Core;

namespace InventoryManagement.WebApi.Endpoints.RoleManagement;

public class GetAllRolePaginated : BaseEndpoint<GetAllRolePaginatedRequest, PagedList<RoleResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllRolePaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("roles")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles paginated",
        Description = "",
        OperationId = "RoleManagement.GetAll",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<UserResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<RoleResponse>>> HandleAsync(
        [FromQuery] GetAllRolePaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Role>().AsQueryable()
            .Where(e => e.RoleId != RoleExtensions.SuperAdministratorId);

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        if (string.IsNullOrWhiteSpace(request.OrderBy))
            request.OrderBy = nameof(Role.CreatedAt);

        if (string.IsNullOrWhiteSpace(request.OrderType))
            request.OrderType = "DESC";

        queryable = queryable.OrderBy($"{request.OrderBy} {request.OrderType}");

        var users = await queryable
            .Select(e => new RoleResponse
            {
                RoleId = e.RoleId,
                Name = e.Name,
                Description = e.Description
            })
            .Skip(request.CalculateSkip())
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var response = new PagedList<RoleResponse>(users, request.Page, request.Size);

        return response;
    }
}