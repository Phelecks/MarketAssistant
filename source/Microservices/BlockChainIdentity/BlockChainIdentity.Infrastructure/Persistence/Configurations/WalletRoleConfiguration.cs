using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class WalletRoleConfiguration : IEntityTypeConfiguration<Domain.Entities.WalletRole>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.WalletRole> builder)
    {
        builder.ToTable("WalletRole");
    }
}
