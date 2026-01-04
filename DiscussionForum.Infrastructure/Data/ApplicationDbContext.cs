using DiscussionForum.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Infrastructure.Data;

/// <summary>
/// Database context for the discussion forum application
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TopicTag> TopicTags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Category
        builder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.HasIndex(e => e.DisplayOrder);
        });

        // Configure Topic
        builder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Content).IsRequired();
            
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Topics)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Topics)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.CreatedDate);
            entity.HasIndex(e => e.CategoryId);
            entity.HasIndex(e => e.UserId);
        });

        // Configure Post
        builder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired();
            
            entity.HasOne(e => e.Topic)
                .WithMany(t => t.Posts)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.QuotedPost)
                .WithMany()
                .HasForeignKey(e => e.QuotedPostId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasIndex(e => e.TopicId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.CreatedDate);
        });

        // Configure Vote
        builder.Entity<Vote>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Post)
                .WithMany(p => p.Votes)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany(u => u.Votes)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();
        });

        // Configure Tag
        builder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Slug).IsUnique();
        });

        // Configure TopicTag (Many-to-Many)
        builder.Entity<TopicTag>(entity =>
        {
            entity.HasKey(e => new { e.TopicId, e.TagId });

            entity.HasOne(e => e.Topic)
                .WithMany(t => t.TopicTags)
                .HasForeignKey(e => e.TopicId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                .WithMany(t => t.TopicTags)
                .HasForeignKey(e => e.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
