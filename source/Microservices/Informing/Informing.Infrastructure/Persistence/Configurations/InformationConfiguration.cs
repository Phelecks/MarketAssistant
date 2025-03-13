using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informing.Infrastructure.Persistence.Configurations;

public class InformationConfiguration : IEntityTypeConfiguration<Information>
{
    public void Configure(EntityTypeBuilder<Information> builder)
    {
        builder.HasIndex(index => index.Title).IsUnique(false);
        //builder.HasIndex(index => index.destination).IsUnique(false);
        builder.ToTable("Information");
    }
}
