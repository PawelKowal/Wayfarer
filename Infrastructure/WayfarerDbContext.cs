using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class WayfarerDbContext: IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReaction> CommentsReactions { get; set; }
        public DbSet<PostReaction> PostsReactions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public WayfarerDbContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(t => t.UserName).HasMaxLength(50);
            builder.Entity<CommentReaction>()
                .HasOne(c => c.User)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<CommentReaction>()
                .HasKey(p => new { p.UserId, p.CommentId });
            builder.Entity<PostReaction>()
                .HasOne(c => c.User)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<PostReaction>()
                .HasKey(p => new { p.UserId, p.PostId });

            builder.Entity<Follow>()
                .HasKey(f => new { f.FollowerId, f.FollowedId });
            builder.Entity<Follow>()
                .HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Follow>()
                .HasOne(f => f.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Chat>()
                .HasOne(c => c.User1)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Chat>()
                .HasOne(c => c.User2)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<ChatMessage>()
                .HasOne(c => c.Author)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
