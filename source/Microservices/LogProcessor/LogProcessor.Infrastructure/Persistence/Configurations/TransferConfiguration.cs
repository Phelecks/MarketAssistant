using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogProcessor.Infrastructure.Persistence.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Domain.Entities.Transfer>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Transfer> builder)
    {
        builder.HasIndex(index => new { index.Hash, index.Chain }).IsUnique(true);
        builder.HasIndex(index => new { index.BlockNumber }).IsUnique(false);
        builder.ToTable("Transfer");
    }
}
