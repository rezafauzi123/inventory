using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Scopes;
using System.Security.Claims;

namespace InventoryManagement.WebApi.Endpoints.Identity.Helpers;

public static class ClaimsGenerator
{
    public static Dictionary<string, IEnumerable<string>> Generate(User user, IAES _iaes)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { user.UserId.ToString() },
            ["usr"] = new[] { user.Username },
            [ClaimTypes.Name] = new[] { user.UserId.ToString() }
        };

        foreach (var userRole in user.UserRoles)
        {
            claims.Add(ClaimTypes.Role, new[] { userRole.RoleId.ToString() });

            if (userRole.Role != null && userRole.Role.RoleScopes.Any())
            {
                claims.Add("scopes", userRole.Role!.RoleScopes.Select(e => e.Name));
            }
        }

        if (!string.IsNullOrWhiteSpace(user.Email))
            claims.Add(ClaimTypes.Email, new[] { user.Email });

        //if normal
        if (claims[ClaimTypes.Role].Any(e => e != RoleExtensions.SuperAdministratorId.ToString()))
            return claims;

        claims.Remove("scopes");

        var _scopes = String.Join(",", ScopeManager.Instance.GetAllScopes());
        var encryptedScopes = _iaes.EncryptStringToBase64String(_scopes);

        claims.Add("scopes", new[] { encryptedScopes });

        return claims;
    }
}