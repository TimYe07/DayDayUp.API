using DayDayUp.BlogContext.Entities.AggregateRoot;
using DayDayUp.BlogContext.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace DayDayUp.BlogContext.Repositories
{
    public class BlogDbContext : DbContext, IUnitOfWork
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Post>()
                .Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();


            modelBuilder.Entity<Tag>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Tag>()
                .Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedNever();


            modelBuilder.Entity<Category>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Id)
                .IsRequired()
                .ValueGeneratedNever();

            modelBuilder.Entity<Category>().HasMany(c => c.Posts)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PostTag>()
                .HasKey(t => new {t.PostId, t.TagId});

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}