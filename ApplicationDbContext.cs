using AlsetTest.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlsetTest;

public class ApplicationDbContext: IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserSubscriber>()
            .HasKey(us => new { us.UserId, us.SubscriberId });

        modelBuilder.Entity<UserSubscriber>()
            .HasOne(uf => uf.User)
            .WithMany(u => u.UserSuscribers)
            .HasForeignKey(uf => uf.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<UserSubscriber>()
            .HasOne(us => us.Subscriber)
            .WithMany()
            .HasForeignKey(uf => uf.SubscriberId)
            .OnDelete(DeleteBehavior.Restrict);
    }
    
    public DbSet<User> User { get; set; }
    public DbSet<UserSubscriber> UserSubscriber { get; set; }
    public DbSet<Journal> Journals { get; set; }
}