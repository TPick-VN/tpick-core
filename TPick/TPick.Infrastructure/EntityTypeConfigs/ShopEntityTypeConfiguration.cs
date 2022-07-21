using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TPick.Domain.Aggregates;

namespace TPick.Infrastructure.EntityTypeConfigs;

public class ShopEntityTypeConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Url);
        builder.Property(x => x.Url);
        builder.Property(x => x.Name);
        builder.Property(x => x.Address);
        builder.Property(x => x.ImageUrl);
        builder.Property(x => x.UpdatedTime);
        builder.Property(x => x.Sections)
            .HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<ShopSection>>(v));
    }
}