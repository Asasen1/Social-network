using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class UserReadModelConfiguration : IEntityTypeConfiguration<UserReadModel>
{
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder
            .HasMany(u => u.Photos)
            .WithOne()
            .IsRequired(false);
        builder
            .HasMany(u => u.Posts)
            .WithOne() 
            .HasForeignKey(u => u.UserId)
            .IsRequired(false);
        builder
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity(j => j.ToTable("user_friends"));
    }
}