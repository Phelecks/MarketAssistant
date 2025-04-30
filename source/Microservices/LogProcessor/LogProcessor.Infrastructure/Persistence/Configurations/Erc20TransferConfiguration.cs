using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogProcessor.Infrastructure.Persistence.Configurations;

public class Erc20TransferConfiguration : IEntityTypeConfiguration<Domain.Entities.Erc20Transfer>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Erc20Transfer> builder)
    {
        builder.ToTable("Erc20Transfer");
    }
}
