using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Header)
            .IsRequired()
            .HasColumnName("header");
        builder
            .Property(p => p.Text)
            .IsRequired()
            .HasColumnName("text");
        builder
            .HasMany(p => p.Photos)
            .WithOne()
            .IsRequired();
        builder
            .HasMany(p => p.Likes)
            .WithOne()
            .IsRequired();
        builder
            .HasMany(p => p.Comments)
            .WithOne()
            .IsRequired();
    }
}