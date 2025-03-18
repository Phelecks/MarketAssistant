using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChain.Infrastructure.Persistence.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Domain.Entities.Track>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Track> builder)
    {
        builder.ToTable("Track");
    }
}
