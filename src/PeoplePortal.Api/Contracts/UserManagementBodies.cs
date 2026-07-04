namespace PeoplePortal.Api.Contracts;

public sealed record CreateUserBody(
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string TempPassword,
    bool   Enabled = true);

public sealed record SetUserEnabledBody(bool Enabled);
public sealed record ResetPasswordBody(string NewPassword, bool Temporary = true);
public sealed record AssignRolesBody(IReadOnlyList<string> RoleNames);
