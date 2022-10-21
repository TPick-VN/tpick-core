#nullable enable
using CsMicro.Core;

namespace TPick.Domain.ValueObjects;

public class SubOrderOwner : ValueObject
{
    public Guid Id { get; }
    public string Name { get; set; } = null!;

    private SubOrderOwner()
    {
    }

    public SubOrderOwner(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return Name;
    }
}