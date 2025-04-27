using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareBook.API.Domain.Entities;

namespace ShareBook.API.Persistence;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Book> Books { get; set; }

    public DbSet<BookInstance> BookInstances { get; set; }

    public DbSet<Library> Libraries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>().HasKey(b => b.Id);

        modelBuilder
            .Entity<Book>()
            .HasOne(b => b.Library)
            .WithMany(l => l.Books)
            .HasForeignKey(b => b.LibraryId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder
            .Entity<Book>()
            .HasMany(b => b.BookInstances)
            .WithOne(bi => bi.Book)
            .HasForeignKey(bi => bi.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookInstance>().HasKey(bi => bi.Id);

        modelBuilder.Entity<Library>().HasKey(l => l.Id);
    }
}
