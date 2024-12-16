using Informing.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Informing.Infrastructure.Persistence.Configurations;

public class GroupInformationConfiguration : IEntityTypeConfiguration<GroupInformation>
{
    public void Configure(EntityTypeBuilder<GroupInformation> builder)
    {
        builder.ToTable("GroupInformation");
    }
}
