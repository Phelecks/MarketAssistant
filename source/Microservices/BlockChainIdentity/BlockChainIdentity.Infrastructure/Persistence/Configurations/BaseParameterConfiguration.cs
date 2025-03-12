using BlockChainIdentity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class BaseParameterConfiguration : IEntityTypeConfiguration<BaseParameter>
{
    public void Configure(EntityTypeBuilder<BaseParameter> builder)
    {
        builder.Property(t => t.Value)
            .HasMaxLength(512)
            .IsRequired();
        builder.Property(t => t.Category)
            .IsRequired();
        builder.Property(t => t.Field)
            .IsRequired();
        builder.ToTable("BaseParameter").HasIndex(index => index.Field).IsUnique();
    }
}
