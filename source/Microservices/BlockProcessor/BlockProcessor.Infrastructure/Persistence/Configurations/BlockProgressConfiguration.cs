using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockProcessor.Infrastructure.Persistence.Configurations;

public class BlockProgressConfiguration : IEntityTypeConfiguration<Domain.Entities.BlockProgress>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.BlockProgress> builder)
    {
        builder.HasIndex(index => new { index.Chain, index.BlockNumber }).IsUnique(false);
        builder.ToTable("BlockProgress");
    }
}
