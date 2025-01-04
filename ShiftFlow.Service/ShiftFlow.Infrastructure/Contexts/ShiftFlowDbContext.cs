using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShiftFlow.Entities;

namespace ShiftFlow.Infrastructure.Contexts;
public class ShiftFlowDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly string? _connectionString;
    public ShiftFlowDbContext()
    {
    }

    public ShiftFlowDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public ShiftFlowDbContext(DbContextOptions<ShiftFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
    }
}

