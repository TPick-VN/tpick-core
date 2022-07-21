using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using TPick.Domain.Aggregates;

namespace TPick.Infrastructure.EntityTypeConfigs;

public class SubOrderEntityTypeConfiguration : IEntityTypeConfiguration<SubOrder>
{
    public void Configure(EntityTypeBuilder<SubOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.OrderId);
        builder.Property(x => x.OrderId);
        builder.Property(x => x.Note);

        builder.OwnsOne(own => own.Owner, value =>
        {
            value.Property(p => p.Id).HasColumnName("OwnerId");
            value.Property(p => p.Name).HasColumnName("OwnerName");
        });

        builder.Property(x => x.Items)
            .HasConversion(v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<SubOrder.OrderItem>>(v));
    }
}