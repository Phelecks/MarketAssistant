using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockProcessor.Infrastructure.Persistence.Configurations;

public class RpcUrlConfiguration : IEntityTypeConfiguration<Domain.Entities.RpcUrl>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.RpcUrl> builder)
    {
        builder.ToTable("RpcUrl");
    }
}
