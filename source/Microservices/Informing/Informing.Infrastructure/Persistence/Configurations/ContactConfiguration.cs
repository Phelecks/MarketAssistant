using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informing.Infrastructure.Persistence.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(t => t.userId)
            .IsRequired();
        builder.Property(t => t.username)
            .IsRequired();
        builder.ToTable("Contact").HasIndex(index => index.userId).IsUnique();
    }
}
