using Domain.Constants;
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
        builder.Property(p => p.Header).IsRequired().HasColumnName("header");
        builder.Property(p => p.Text).IsRequired().HasColumnName("text");
        // builder.ComplexProperty(p => p.AuthorId, a =>
        // {
        //     a.Property(f => f.FirstName)
        //         .HasColumnName("first_name")
        //         .IsRequired()
        //         .HasMaxLength(UserConstants.MAXIMUM_LENGTH_NAME);
        //     a.Property(f => f.SecondName)
        //         .HasColumnName("last_name")
        //         .IsRequired()
        //         .HasMaxLength(UserConstants.MAXIMUM_LENGTH_NAME);
        // });
        builder.HasMany(p => p.Photos).WithOne().IsRequired(false);
        builder.HasOne(p => p.Author)
            .WithMany()
            .HasForeignKey(p => p.Id);
    }
}