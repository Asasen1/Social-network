using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class UserPhotoReadModelConfiguration : IEntityTypeConfiguration<UserPhotoReadModel>
{
    public void Configure(EntityTypeBuilder<UserPhotoReadModel> builder)
    {
        builder.ToTable("user_photos");
        builder.HasKey(u => u.Id);
        builder
            .HasOne<UserReadModel>()
            .WithMany()
            .HasForeignKey(u => u.UserId);
        // builder
        //     .ToTable("likes")
        //     .HasMany(u => u.Likes)
        //     .WithOne().IsRequired();
    }
}