using InventoryManagement.FunctionalTests.Helpers;
using InventoryManagement.Shared.Abstractions.Serialization;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Identity.Requests;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit.Abstractions;

namespace InventoryManagement.FunctionalTests.Endpoints.Identity;

public class SignInFlowTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _waf;
    private readonly HttpClient _client;

    public SignInFlowTests(CustomWebApplicationFactory waf, ITestOutputHelper testOutputHelper)
    {
        waf.SetOutPut(testOutputHelper);
        _client = waf.CreateClient();
        _waf = waf;
    }

    [Fact]
    public async Task SignIn_RefreshToken_Should_Do_As_Expected()
    {
        const string getMePath = "api/identity/me";
        const string refreshTokenPath = "api/identity/refresh";

        //first we do login
        var token = await _client.LoginAsync("admin", "Qwerty@1234");

        // use to get me api
        var getMeRequest = HttpRequestMessageExtensions.Create(HttpMethod.Get,
            _client.BaseAddress!,
            getMePath,
            null,
            new Dictionary<string, string>
            {
                { "Authorization", $"bearer {token.AccessToken}" }
            });

        // Act
        var responseGetMe = await _client.SendAsync(getMeRequest);
        responseGetMe.StatusCode.ShouldBe(HttpStatusCode.OK);

        var refreshTokenRequest = HttpRequestMessageExtensions.Create(HttpMethod.Post,
            _client.BaseAddress!,
            refreshTokenPath,
            HttpRequestMessageExtensions.CreateJsonAsContent(new RefreshTokenRequest
            {
                RefreshToken = token.RefreshToken
            }),
            new Dictionary<string, string>
            {
                { "Authorization", $"bearer {token.AccessToken}" }
            });

        //response refresh token
        var responseRefreshToken = await _client.SendAsync(refreshTokenRequest);
        responseRefreshToken.StatusCode.ShouldBe(HttpStatusCode.OK);
        var responseInString = await responseRefreshToken.Content.ReadAsStringAsync();
        var secondToken = _waf.Services.GetRequiredService<IJsonSerializer>()
            .Deserialize<LoginResponse>(responseInString);
        secondToken.ShouldNotBeNull();

        getMeRequest = HttpRequestMessageExtensions.Create(HttpMethod.Get,
            _client.BaseAddress!,
            getMePath,
            null,
            new Dictionary<string, string>
            {
                { "Authorization", $"bearer {secondToken.AccessToken}" }
            });
        var responseThirdGetMe = await _client.SendAsync(getMeRequest);
        responseInString = await responseThirdGetMe.Content.ReadAsStringAsync();
        _waf.Output!.WriteLine(responseInString);

        responseThirdGetMe.StatusCode.ShouldBe(HttpStatusCode.OK);

        var getMeResponse = _waf.Services.GetRequiredService<IJsonSerializer>()
            .Deserialize<UserResponse>(responseInString);
        getMeResponse.ShouldNotBeNull();
    }
}