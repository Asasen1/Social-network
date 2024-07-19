using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class UserPhotoConfiguration : IEntityTypeConfiguration<UserPhoto>
{
    public void Configure(EntityTypeBuilder<UserPhoto> builder)
    {
        builder.ToTable("user_photos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Path).IsRequired().HasColumnName("path");
        builder.Property(p => p.IsMain).IsRequired().HasColumnName("is_main");
    }
}