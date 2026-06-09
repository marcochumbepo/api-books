using Microsoft.EntityFrameworkCore;
using MsBooks.Domain.Entities;

namespace MsBooks.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(b => b.Description)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(b => b.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);
        });
    }
}
