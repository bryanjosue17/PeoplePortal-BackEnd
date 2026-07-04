using PeoplePortal.Application.Users.Dtos;

namespace PeoplePortal.Application.Contracts.Services;

public interface IKeycloakAdminService
{
    Task<IReadOnlyList<KeycloakUserDto>>  GetUsersAsync(CancellationToken ct = default);
    Task<KeycloakUserDto>                 CreateUserAsync(CreateKeycloakUserRequest req, CancellationToken ct = default);
    Task                                  SetUserEnabledAsync(string userId, bool enabled, CancellationToken ct = default);
    Task                                  ResetPasswordAsync(string userId, string newPassword, bool temporary, CancellationToken ct = default);
    Task<IReadOnlyList<KeycloakRoleDto>>  GetRealmRolesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<KeycloakRoleDto>>  GetUserRolesAsync(string userId, CancellationToken ct = default);
    Task                                  AssignRolesAsync(string userId, IReadOnlyList<KeycloakRoleDto> roles, CancellationToken ct = default);
    Task                                  RemoveRolesAsync(string userId, IReadOnlyList<KeycloakRoleDto> roles, CancellationToken ct = default);
}

public sealed record CreateKeycloakUserRequest(
    string  Username,
    string  Email,
    string  FirstName,
    string  LastName,
    string  TempPassword,
    bool    Enabled = true);
