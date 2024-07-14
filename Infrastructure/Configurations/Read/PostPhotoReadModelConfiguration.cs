using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class PostPhotoReadModelConfiguration : IEntityTypeConfiguration<PostPhotoReadModel>
{
    public void Configure(EntityTypeBuilder<PostPhotoReadModel> builder)
    {
        builder.ToTable("post_photos");
        builder.HasKey(p => p.Id);
        builder
            .HasMany<PostPhotoReadModel>()
            .WithOne()
            .HasForeignKey(p => p.PostId);
    }
}