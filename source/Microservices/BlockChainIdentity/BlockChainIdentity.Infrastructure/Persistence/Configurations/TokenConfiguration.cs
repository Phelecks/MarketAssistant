using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlockChainIdentity.Infrastructure.Persistence.Configurations;

public class TokenConfiguration : IEntityTypeConfiguration<Domain.Entities.Token>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Token> builder)
    {
        builder.ToTable("Token");
    }
}
