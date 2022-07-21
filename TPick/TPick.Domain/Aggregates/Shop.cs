using CsMicro.Core;

namespace TPick.Domain.Aggregates;

public class Shop : AggregateRoot<Guid>
{
    public override Guid Id { get; init; }
    public string Url { get; }
    public string Name { get; private set;}
    public string ImageUrl { get; private set;}
    public string Address { get; private set;}
    public List<ShopSection> Sections { get; } = new();
    public DateTimeOffset? UpdatedTime { get; private set; }

    public Shop(string url)
    {
        Url = url;
        Name = string.Empty;
        ImageUrl = string.Empty;
        Address = string.Empty;
    }

    public void OverrideDetails(List<ShopSection> sections, string name, string imageUrl, string address, DateTimeOffset updatedTime)
    {
        Name = name;
        ImageUrl = imageUrl;
        Address = address;
        UpdatedTime = updatedTime;
        Sections.Clear();
        Sections.AddRange(sections);
    }
}

public class ShopSection
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ShopItem> Items { get; set; }
}

public class ShopItem
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set;}
    public bool IsAvailable { get; set; }
}