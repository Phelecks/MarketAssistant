using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informing.Infrastructure.Persistence.Configurations;

public class InformationLogConfiguration : IEntityTypeConfiguration<InformationLog>
{
    public void Configure(EntityTypeBuilder<InformationLog> builder)
    {
        builder.ToTable("InformationLog");
    }
}
