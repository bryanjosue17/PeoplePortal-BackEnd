using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PeoplePortal.Application.Contracts.Services;
using PeoplePortal.Application.Users.Dtos;

namespace PeoplePortal.Infrastructure.Services;

public sealed class KeycloakAdminService(IOptions<KeycloakAdminOptions> opts) : IKeycloakAdminService
{
    private readonly KeycloakAdminOptions _o = opts.Value;
    private readonly HttpClient _http = new();
    private string?   _token;
    private DateTime  _tokenExpiry = DateTime.MinValue;

    // ── Token management ─────────────────────────────────────────────────
    private async Task<string> GetTokenAsync(CancellationToken ct)
    {
        if (_token is not null && DateTime.UtcNow < _tokenExpiry)
            return _token;

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "password",
            ["client_id"]  = "admin-cli",
            ["username"]   = _o.AdminUsername,
            ["password"]   = _o.AdminPassword,
        });
        var res = await _http.PostAsync($"{_o.BaseUrl}/realms/master/protocol/openid-connect/token", form, ct);
        res.EnsureSuccessStatusCode();

        using var doc  = await JsonDocument.ParseAsync(await res.Content.ReadAsStreamAsync(ct), cancellationToken: ct);
        _token         = doc.RootElement.GetProperty("access_token").GetString()!;
        var expiresIn  = doc.RootElement.GetProperty("expires_in").GetInt32();
        _tokenExpiry   = DateTime.UtcNow.AddSeconds(expiresIn - 30);
        return _token;
    }

    private async Task SetAuthHeaderAsync(CancellationToken ct)
    {
        var token = await GetTokenAsync(ct);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // ── Users ─────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<KeycloakUserDto>> GetUsersAsync(CancellationToken ct = default)
    {
        await SetAuthHeaderAsync(ct);
        var users = await _http.GetFromJsonAsync<List<JsonElement>>(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users?max=500", ct) ?? [];

        return users.Select(MapUser).ToArray();
    }

    public async Task<KeycloakUserDto> CreateUserAsync(CreateKeycloakUserRequest req, CancellationToken ct = default)
    {
        await SetAuthHeaderAsync(ct);

        var payload = new
        {
            username    = req.Username,
            email       = req.Email,
            firstName   = req.FirstName,
            lastName    = req.LastName,
            enabled     = req.Enabled,
            credentials = new[]
            {
                new { type = "password", value = req.TempPassword, temporary = true }
            }
        };

        var res = await _http.PostAsJsonAsync(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users", payload, ct);
        res.EnsureSuccessStatusCode();

        // Keycloak returns 201 with Location header pointing to the new user
        var location = res.Headers.Location?.ToString()
            ?? throw new InvalidOperationException("Keycloak did not return Location header.");
        var newId = location.Split('/').Last();

        // Fetch the created user
        var created = await _http.GetFromJsonAsync<JsonElement>(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{newId}", ct);
        return MapUser(created);
    }

    public async Task SetUserEnabledAsync(string userId, bool enabled, CancellationToken ct = default)
    {
        await SetAuthHeaderAsync(ct);
        var res = await _http.PutAsJsonAsync(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{userId}",
            new { enabled }, ct);
        res.EnsureSuccessStatusCode();
    }

    public async Task ResetPasswordAsync(string userId, string newPassword, bool temporary, CancellationToken ct = default)
    {
        await SetAuthHeaderAsync(ct);
        var res = await _http.PutAsJsonAsync(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{userId}/reset-password",
            new { type = "password", value = newPassword, temporary }, ct);
        res.EnsureSuccessStatusCode();
    }

    // ── Roles ─────────────────────────────────────────────────────────────
    public async Task<IReadOnlyList<KeycloakRoleDto>> GetRealmRolesAsync(CancellationToken ct = default)
    {
        await SetAuthHeaderAsync(ct);
        var roles = await _http.GetFromJsonAsync<List<JsonElement>>(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/roles", ct) ?? [];

        return roles
            .Where(r => !r.GetProperty("name").GetString()!.StartsWith("default-roles") &&
                        !r.GetProperty("name").GetString()!.StartsWith("offline_access") &&
                        !r.GetProperty("name").GetString()!.StartsWith("uma_authorization"))
            .Select(MapRole)
            .OrderBy(r => r.Name)
            .ToArray();
    }

    public async Task<IReadOnlyList<KeycloakRoleDto>> GetUserRolesAsync(string userId, CancellationToken ct = default)
    {
        var roles = await _http.GetFromJsonAsync<List<JsonElement>>(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{userId}/role-mappings/realm", ct) ?? [];
        return roles.Select(MapRole).ToArray();
    }

    public async Task AssignRolesAsync(string userId, IReadOnlyList<KeycloakRoleDto> roles, CancellationToken ct = default)
    {
        if (roles.Count == 0) return;
        await SetAuthHeaderAsync(ct);
        var res = await _http.PostAsJsonAsync(
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{userId}/role-mappings/realm",
            roles.Select(r => new { id = r.Id, name = r.Name }), ct);
        res.EnsureSuccessStatusCode();
    }

    public async Task RemoveRolesAsync(string userId, IReadOnlyList<KeycloakRoleDto> roles, CancellationToken ct = default)
    {
        if (roles.Count == 0) return;
        await SetAuthHeaderAsync(ct);
        var req = new HttpRequestMessage(HttpMethod.Delete,
            $"{_o.BaseUrl}/admin/realms/{_o.Realm}/users/{userId}/role-mappings/realm")
        {
            Content = JsonContent.Create(roles.Select(r => new { id = r.Id, name = r.Name }))
        };
        var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();
    }

    // ── Helpers ───────────────────────────────────────────────────────────
    private static KeycloakUserDto MapUser(JsonElement e) => new(
        e.GetProperty("id").GetString()!,
        e.TryGetProperty("username",  out var u) ? u.GetString() : null,
        e.TryGetProperty("email",     out var em) ? em.GetString() : null,
        e.TryGetProperty("firstName", out var fn) ? fn.GetString() : null,
        e.TryGetProperty("lastName",  out var ln) ? ln.GetString() : null,
        e.TryGetProperty("enabled",   out var en) && en.GetBoolean(),
        e.TryGetProperty("createdTimestamp", out var ct2) ? ct2.GetInt64() : null);

    private static KeycloakRoleDto MapRole(JsonElement e) => new(
        e.GetProperty("id").GetString()!,
        e.GetProperty("name").GetString()!,
        e.TryGetProperty("description", out var d) ? d.GetString() : null);
}



