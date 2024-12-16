using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class ResourceConfiguration : IEntityTypeConfiguration<Domain.Entities.Resource>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Resource> builder)
    {
        builder.HasIndex(index => index.title).IsUnique(true);
        builder.ToTable("Resource");
    }
}
