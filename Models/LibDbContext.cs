using Microsoft.EntityFrameworkCore;

namespace LibManager.Models;

public class LibDbContext : DbContext
{
    public LibDbContext(DbContextOptions<LibDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { set; get; } = default!;
    public DbSet<Book> Books { set; get; } = default!;
    public DbSet<Borrowing> Borrowings { set; get; } = default!;
    public DbSet<Report> Reports { set; get; } = default!;
    public DbSet<Category> Categories { set; get; } = default!;
    public DbSet<Notication> Notications { set; get; } = default!;




}