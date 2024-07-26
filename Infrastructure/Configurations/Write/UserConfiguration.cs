using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.HasKey(u => u.Id);
        builder.ComplexProperty(v => v.FullName, b =>
        {
            b.Property(f => f.FirstName)
                .HasColumnName("first_name")
                .IsRequired()
                .HasMaxLength(UserConstants.MAX_LENGTH_NAME);
                b.Property(f => f.SecondName)
                    .HasColumnName("second_name")
                    .IsRequired()
                .HasMaxLength(UserConstants.MAX_LENGTH_NAME);
        });
        builder.Property(u => u.Nickname).IsRequired().HasColumnName("nickname");
        builder.Property(u => u.BirthDate).IsRequired(false).HasColumnName("birth_date");
        builder.Property(u => u.Description).IsRequired(false).HasColumnName("description");
        builder.Property(u => u.CreatedDate).IsRequired().HasColumnName("created_date");
        builder.HasMany(u => u.Photos).WithOne().IsRequired();
        builder.HasMany(u => u.Posts).WithOne().IsRequired();
        builder
            .HasMany(u => u.Friends)
            .WithMany()
            .UsingEntity(j => j.ToTable("user_friends"));
    }
}