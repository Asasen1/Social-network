using Domain.Entities.Photos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class PostPhotoConfiguration : IEntityTypeConfiguration<PostPhoto>
{
    public void Configure(EntityTypeBuilder<PostPhoto> builder)
    {
        builder.ToTable("post_photos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Path).IsRequired().HasColumnName("path");
        builder.Property(p => p.IsMain).IsRequired().HasColumnName("is_main");
    }
}