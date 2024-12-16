using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Domain.Entities.Client>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Client> builder)
    {
        builder.HasIndex(index => index.clientId).IsUnique(true);
        builder.ToTable("Client");
    }
}
