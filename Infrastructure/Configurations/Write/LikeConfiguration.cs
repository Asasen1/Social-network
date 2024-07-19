using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasKey(l => l.Id);
        // builder.HasOne(l => l.Post).WithMany().IsRequired();
        // builder.HasOne(l => l.User).WithMany().IsRequired();
    }
}