using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class LikeReadModelConfiguration : IEntityTypeConfiguration<LikeReadModel>
{
    public void Configure(EntityTypeBuilder<LikeReadModel> builder)
    {
        builder.ToTable("likes");
        builder.HasKey(l => l.Id);
        builder
            .HasOne<PostReadModel>()
            .WithMany()
            .HasForeignKey(l => l.PostId);
        builder
            .HasOne<UserReadModel>()
            .WithMany()
            .HasForeignKey(l => l.UserId);
    }
}