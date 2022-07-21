using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Linq;
using TPick.Domain.Aggregates;

namespace TPick.Infrastructure.EntityTypeConfigs;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ShopId);
        builder.OwnsOne(own => own.Host, value => {
            value.Property(p => p.Id).HasColumnName("HostId");
            value.Property(p => p.Name).HasColumnName("HostName");
        });
        builder.Property(x => x.Fee).HasConversion(
            to => to.ToString(),
            from => JObject.Parse(from));
        builder.Property(x => x.Discount).HasConversion(
            to => to.ToString(),
            from => JObject.Parse(from));
    }
}