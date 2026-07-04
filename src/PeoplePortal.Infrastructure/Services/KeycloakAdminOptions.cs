namespace PeoplePortal.Infrastructure.Services;

public sealed class KeycloakAdminOptions
{
    public string BaseUrl        { get; set; } = string.Empty;
    public string Realm          { get; set; } = "peopleportal";
    public string AdminUsername  { get; set; } = "admin";
    public string AdminPassword  { get; set; } = string.Empty;
}
