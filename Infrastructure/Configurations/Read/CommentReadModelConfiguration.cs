using Infrastructure.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Read;

public class CommentReadModelConfiguration : IEntityTypeConfiguration<CommentReadModel>
{
    public void Configure(EntityTypeBuilder<CommentReadModel> builder)
    {
        builder.ToTable("comments");
        builder.HasKey(c => c.Id);
        builder
            .Property(c => c.Text)
            .HasColumnName("text");
        builder
            .HasOne<PostReadModel>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.PostId);
        builder
            .HasOne<UserReadModel>()
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.UserId);
    }
}