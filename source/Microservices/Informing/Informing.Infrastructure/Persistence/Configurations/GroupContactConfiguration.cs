using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informing.Infrastructure.Persistence.Configurations;

public class GroupContactConfiguration : IEntityTypeConfiguration<GroupContact>
{
    public void Configure(EntityTypeBuilder<GroupContact> builder)
    {
        builder.ToTable("GroupContact");
    }
}
