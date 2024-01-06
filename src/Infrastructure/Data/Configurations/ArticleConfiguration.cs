using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();
        
        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
        builder.HasIndex(t => t.Title).IsUnique();
        builder.Property(t => t.Slug)
            .HasMaxLength(200)
            .IsRequired();
        builder.HasIndex(t => t.Slug).IsUnique();
        builder.Property(t => t.ShortDescription)
            .HasMaxLength(500);
        builder.Property(t => t.DisplayImage)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(t => t.Content)
            .IsRequired()
            .HasColumnType("text");
        builder.Property(t => t.MetaDescription)
            .HasColumnType("text");
 
        builder.HasMany(t => t.Tags)
        .WithMany(t => t.Articles)
        .UsingEntity(j => j.ToTable("ArticleTags"));




    }
}
