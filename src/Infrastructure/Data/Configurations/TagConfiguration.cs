using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.Name)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(t => t.Description)
            .HasMaxLength(200);

        builder.HasIndex(t => t.Name).IsUnique();
        builder.Property(t => t.Slug)
            .HasMaxLength(20)
            .IsRequired();
        builder.HasIndex(t => t.Slug).IsUnique();
    }
}
