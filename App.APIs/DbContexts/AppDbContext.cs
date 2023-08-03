using App.APIs.Models;
using Microsoft.EntityFrameworkCore;

namespace App.APIs.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products;

}