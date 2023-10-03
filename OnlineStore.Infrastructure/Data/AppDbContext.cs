using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Entites;

namespace OnlineStore.Infrastructure.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
    {

    }
    public DbSet<User> User { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Order> Order { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
           .Property(b => b.CreationDate)
         .HasDefaultValueSql("getdate()");


        modelBuilder.Entity<User>().HasData(
         new User
         {
             Id = Guid.NewGuid(),
             Name = "user1"
         },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "user2"
            },
               new User
               {
                   Id = Guid.NewGuid(),
                   Name = "user3"
               }
         );
    }
}
