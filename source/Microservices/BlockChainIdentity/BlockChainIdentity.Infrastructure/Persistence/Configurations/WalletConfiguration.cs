using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Domain.Entities.Wallet>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Wallet> builder)
    {
        builder.HasIndex(index => new { index.Address, index.ChainId }).IsUnique(true);
        builder.ToTable("Wallet");
    }
}
