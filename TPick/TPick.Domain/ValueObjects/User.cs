using CsMicro.Core;

namespace TPick.Domain.ValueObjects;

public class User : ValueObject
{
    public Guid Id { get; }
    public string Name { get; } = null!;

    private User()
    {
    }

    public User(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield  return Id;
        yield  return Name;
    }
}