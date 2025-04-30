using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LogProcessor.Infrastructure.Persistence.Configurations;

public class RpcUrlConfiguration : IEntityTypeConfiguration<Domain.Entities.RpcUrl>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.RpcUrl> builder)
    {
        builder.ToTable("RpcUrl");
    }
}
