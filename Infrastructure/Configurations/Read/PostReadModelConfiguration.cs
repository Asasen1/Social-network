using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class PostReadModelConfiguration : IEntityTypeConfiguration<PostReadModel>
{
    public void Configure(EntityTypeBuilder<PostReadModel> builder)
    {
        builder.ToTable("posts");
        builder.HasKey(p => p.Id);
        builder
            .HasMany(p => p.Photos)
            .WithOne()
            .IsRequired(false);
        builder
            .HasOne<UserReadModel>()
            .WithMany()
            .HasForeignKey(p => p.UserId);
    }
}