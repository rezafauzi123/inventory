using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Endpoints.RoleManagement;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit.Abstractions;

namespace InventoryManagement.IntegrationTests.Endpoints.RoleManagement;

[Collection(nameof(RoleManagementFixture))]
public class EditRoleTests
{
    private readonly RoleManagementFixture _fixture;

    public EditRoleTests(RoleManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task EditRole_Given_CorrectRequest_Should_Be_Correct()
    {
        // Arrange
        using var scope = _fixture.ServiceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        var editRole = new EditRole(
            scope.ServiceProvider.GetRequiredService<IRoleService>(),
            dbContext,
            scope.ServiceProvider.GetRequiredService<IStringLocalizer<EditRole>>());

        var request = new EditRoleRequest
        {
            RoleId = RoleExtensions.SuperAdministratorId,
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test",
                Scopes = new List<string>
                {
                    new UserManagementScope().ScopeName
                }
            }
        };

        // Act
        var result = await editRole.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var role = await dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(CancellationToken.None);
        role.ShouldNotBeNull();
        role.Description.ShouldBe(request.Payload.Description);

        var roleScope = role.RoleScopes.FirstOrDefault();
        roleScope.ShouldNotBeNull();
        roleScope.Name.ShouldBe(new UserManagementScope().ScopeName);
    }
}