using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogProcessor.Infrastructure.Persistence.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Domain.Entities.Token>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Token> builder)
    {
        builder.Property(t => t.ContractAddress)
            .HasMaxLength(512)
            .IsRequired();
        builder.ToTable("Token").HasIndex(index => index.ContractAddress).IsUnique(false);
    }
}
