namespace PeoplePortal.Domain.Entities;

public class Benefit
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private Benefit()
    {
    }

    public static Benefit Create(string name, string type, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type is required.", nameof(type));

        return new Benefit
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Type = type,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
