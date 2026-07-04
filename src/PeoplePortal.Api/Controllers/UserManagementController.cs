using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeoplePortal.Api.Contracts;
using PeoplePortal.Application.Contracts.Persistence;
using PeoplePortal.Application.Contracts.Services;
using PeoplePortal.Application.Users.Dtos;

namespace PeoplePortal.Api.Controllers;

[ApiController]
[Route("api/hr/users")]
[Authorize(Policy = "HrPolicy")]
public class UserManagementController(
    IKeycloakAdminService keycloak,
    IEmployeeRepository   employees) : ControllerBase
{
    // ── GET /api/hr/users ─────────────────────────────────────────────────
    [HttpGet]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var kcUsers   = await keycloak.GetUsersAsync(ct);
        var empList   = await employees.GetAllAsync(ct);
        var empByKcId = empList.ToDictionary(e => e.KeycloakId, StringComparer.OrdinalIgnoreCase);

        var profiles = new List<UserProfileDto>(kcUsers.Count);
        foreach (var u in kcUsers)
        {
            var roles    = await keycloak.GetUserRolesAsync(u.Id, ct);
            empByKcId.TryGetValue(u.Id, out var emp);
            profiles.Add(new UserProfileDto(
                u.Id, u.Username, u.Email, u.FirstName, u.LastName,
                u.Enabled, u.CreatedTimestamp,
                roles.Select(r => r.Name).ToArray(),
                emp?.Id, emp?.Code, emp?.FullName, emp?.Department, emp?.Position,
                emp?.Status.ToString()));
        }

        return Ok(profiles);
    }

    // ── GET /api/hr/users/roles ───────────────────────────────────────────
    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles(CancellationToken ct)
    {
        var roles = await keycloak.GetRealmRolesAsync(ct);
        return Ok(roles);
    }

    // ── GET /api/hr/users/{id}/roles ──────────────────────────────────────
    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetUserRoles(string id, CancellationToken ct)
    {
        var roles = await keycloak.GetUserRolesAsync(id, ct);
        return Ok(roles);
    }

    // ── POST /api/hr/users ────────────────────────────────────────────────
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserBody body, CancellationToken ct)
    {
        var req    = new CreateKeycloakUserRequest(body.Username, body.Email, body.FirstName, body.LastName, body.TempPassword, body.Enabled);
        var result = await keycloak.CreateUserAsync(req, ct);
        return CreatedAtAction(nameof(GetUsers), new { id = result.Id }, result);
    }

    // ── PATCH /api/hr/users/{id}/enabled ──────────────────────────────────
    [HttpPatch("{id}/enabled")]
    public async Task<IActionResult> SetEnabled(string id, [FromBody] SetUserEnabledBody body, CancellationToken ct)
    {
        await keycloak.SetUserEnabledAsync(id, body.Enabled, ct);
        return NoContent();
    }

    // ── POST /api/hr/users/{id}/reset-password ────────────────────────────
    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordBody body, CancellationToken ct)
    {
        await keycloak.ResetPasswordAsync(id, body.NewPassword, body.Temporary, ct);
        return NoContent();
    }

    // ── PUT /api/hr/users/{id}/roles ──────────────────────────────────────
    [HttpPut("{id}/roles")]
    public async Task<IActionResult> SetRoles(string id, [FromBody] AssignRolesBody body, CancellationToken ct)
    {
        var allRoles     = await keycloak.GetRealmRolesAsync(ct);
        var currentRoles = await keycloak.GetUserRolesAsync(id, ct);

        var toAssign = allRoles.Where(r => body.RoleNames.Contains(r.Name)).ToArray();
        var toRemove = currentRoles.Where(r => !body.RoleNames.Contains(r.Name)).ToArray();

        if (toRemove.Length > 0) await keycloak.RemoveRolesAsync(id, toRemove, ct);
        if (toAssign.Length > 0) await keycloak.AssignRolesAsync(id, toAssign, ct);

        return NoContent();
    }
}
