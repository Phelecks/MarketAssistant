using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class ClientResourceConfiguration : IEntityTypeConfiguration<Domain.Entities.ClientResource>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.ClientResource> builder)
    {
        builder.ToTable("ClientResource");
    }
}
