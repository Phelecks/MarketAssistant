using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockProcessor.Infrastructure.Persistence.Configurations;

public class WalletAddressConfiguration : IEntityTypeConfiguration<Domain.Entities.WalletAddress>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.WalletAddress> builder)
    {
        builder.Property(t => t.Address)
            .HasMaxLength(512)
            .IsRequired();
        builder.ToTable("WalletAddress").HasIndex(index => index.Address).IsUnique();
    }
}
