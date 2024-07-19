using Domain.Constants;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Write;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Text).HasMaxLength(200).HasColumnName("text");
        // builder.HasOne(c => c.Post).WithMany();
        // builder.HasOne(c => c.User).WithMany();
    }
}