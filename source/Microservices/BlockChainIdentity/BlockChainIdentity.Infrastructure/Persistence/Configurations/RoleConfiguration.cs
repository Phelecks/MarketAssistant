using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Domain.Entities.Role>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Role> builder)
    {
        builder.HasIndex(index => index.Title).IsUnique(true);
        builder.ToTable("Role");
    }
}
