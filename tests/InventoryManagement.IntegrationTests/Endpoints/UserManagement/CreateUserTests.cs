using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Endpoints.UserManagement;
using InventoryManagement.WebApi.Endpoints.UserManagement.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit.Abstractions;

namespace InventoryManagement.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class CreateUserTests
{
    private readonly UserManagementFixture _fixture;

    public CreateUserTests(UserManagementFixture fixture, ITestOutputHelper testOutputHelper)
    {
        fixture.SetOutput(testOutputHelper);
        fixture.ConstructFixture();
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateUser_Given_CorrectRequest_With_CorrectValue_ShouldReturn_NoContent()
    {
        // Arrange
        var userService = _fixture.ServiceProvider.GetRequiredService<IUserService>();

        var createUser = new CreateUser(
            _fixture.ServiceProvider.GetRequiredService<IDbContext>(),
            userService,
            _fixture.ServiceProvider.GetRequiredService<IRng>(),
            _fixture.ServiceProvider.GetRequiredService<ISalter>(),
            _fixture.ServiceProvider.GetRequiredService<IStringLocalizer<CreateUser>>());

        var request = new CreateUserRequest
        {
            Username = "admin2",
            Password = "Test@12345",
            Fullname = "Super Administrator",
            RoleId = RoleExtensions.SuperAdministratorId,
            EmailAddress = "test@test.com"
        };

        // Act
        var result = await createUser.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var user = await userService.GetByUsernameAsync(request.Username, CancellationToken.None);
        user.ShouldNotBeNull();
        user.NormalizedUsername.ShouldBe(request.Username.ToUpper());
    }
}