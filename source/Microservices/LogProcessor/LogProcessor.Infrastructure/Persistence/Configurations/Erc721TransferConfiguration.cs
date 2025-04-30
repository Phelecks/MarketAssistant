using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogProcessor.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Domain.Entities.Erc721Transfer>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Erc721Transfer> builder)
    {
        builder.ToTable("Erc721Transfer");
    }
}
